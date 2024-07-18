using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Sabanishi.SdiAssignment
{
    /// <summary>
    /// 登録されているリポジトリを監視し、変更が有った場合に通知を受け取るクラス
    /// </summary>
    public class GitRepoObserver:MonoBehaviour
    {
        [SerializeField] private AnimatorController animatorController;
        
        private List<GitRepo> _repos;

        private const float AnimationTime = 3.0f;
        private float _animationTime;

        private void Start()
        {
            _repos = new List<GitRepo>();
            SaveSubject.Instance.SaveObservable.Subscribe(x =>
            {
                if (x != typeof(GitRepoData)) return;
                UpdateObserveRepos();
            }).AddTo(gameObject);
            
            UpdateObserveRepos();
        }

        private void Update()
        {
            if (_animationTime > 0)
            {
                _animationTime -= Time.deltaTime;
                if (_animationTime <= 0)
                {
                    animatorController.SetIsDance(false);
                }
            }
            
            foreach (var repo in _repos)
            {
                repo.Update();
            }
        }

        private void StartDance()
        {
            _animationTime = AnimationTime;
            animatorController.SetIsDance(true);
        }

        private void UpdateObserveRepos()
        {
            if (!SaveUtils.TryLoad<GitRepoData>(out var data))
            {
                data = new GitRepoData();
            }

            foreach (var repoPath in data.repoPaths)
            {
                if(_repos.Exists(x => x.RepoPath.Equals(repoPath))) continue;
                var repo = new GitRepo(repoPath);
                repo.UpdateObservable.Subscribe(_ =>
                {
                    StartDance();
                }).AddTo(gameObject);
                _repos.Add(repo);
            }
            
            foreach(var repo in _repos)
            {
                if(data.repoPaths.Contains(repo.RepoPath)) continue;
                repo.Dispose();
            }
            _repos.RemoveAll(x => !data.repoPaths.Contains(x.RepoPath));
        }
    }
}