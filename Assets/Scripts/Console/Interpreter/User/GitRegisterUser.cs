using System;
using Cysharp.Threading.Tasks;
using LibGit2Sharp;
using UnityEngine;

namespace Sabanishi.SdiAssignment
{
    [Command(name:"!git_repo",description:"Register Git Repository")]
    public class GitRegisterUser:IRunnable
    {
        [Option(names:new[]{"-p","--path"},description:"Repository Path")]
        public static string path;
        
        public async UniTask<bool> Run()
        {
            await UniTask.Delay(500);
            
            var isExistRepo = IsExistRepo();
            if (!isExistRepo)
            {
                Outputter.Instance.Output("そこにリポジトリは無いと思うよ");
                return true;
            }
            
            // リポジトリを登録
            if (!SaveUtils.TryLoad<GitRepoData>(out var data))
            {
                data = new GitRepoData();
            }
            if(data.repoPaths.Contains(path))
            {
                Outputter.Instance.Output("このリポジトリは既に登録してるよ！");
                return true;
            }
            data.repoPaths.Add(path);
            
            SaveUtils.TrySave(data);
            
            Outputter.Instance.Output("リポジトリを登録しました！");
            return true;
        }

        private bool IsExistRepo()
        {
            try
            {
                using (var repo = new Repository(path))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
                return false;
            }
        }
    }
}