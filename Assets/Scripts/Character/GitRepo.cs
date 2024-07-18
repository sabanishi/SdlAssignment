using System;
using LibGit2Sharp;
using UniRx;
using UnityEngine;

namespace Sabanishi.SdiAssignment
{
    public class GitRepo
    {
        public readonly string RepoPath;
        private string _catchCommitSha;
        private string _currentBranch;
        
        private readonly Subject<Unit> _updateSubject;
        public IObservable<Unit> UpdateObservable => _updateSubject;

        private const float Interval = -1;
        private float _catch;

        public GitRepo(string path)
        {
            RepoPath = path;
            var catchCommit = GetLatestCommit();
            _catchCommitSha = catchCommit.sha;
            _currentBranch = catchCommit.branch;
            _updateSubject = new Subject<Unit>();
        }
        
        public void Dispose()
        {
            _updateSubject.Dispose();
        }

        public void Update()
        {
            var currentCommit = GetLatestCommit();
            if (currentCommit.sha.Equals(_catchCommitSha)) return;
            
            _catchCommitSha = currentCommit.sha;

            if (currentCommit.branch.Equals(_currentBranch))
            {
                _updateSubject.OnNext(Unit.Default);
            }
            else
            {
                _currentBranch = currentCommit.branch;
            }
        }
        
        private (string sha,string branch) GetLatestCommit()
        {
            try
            {
                using (var repo = new Repository(RepoPath))
                {
                    var sha = repo.Head.Tip.Sha;
                    var branch = repo.Head.FriendlyName;
                    return (sha, branch);
                }
            }
            catch (Exception e)
            {
                return ("", "");
            }
        }
    }
}