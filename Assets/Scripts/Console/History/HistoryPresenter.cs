using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Sabanishi.SdiAssignment
{
    public class HistoryPresenter:IScopable
    {
        private readonly HistoryModel _model;
        private readonly HistoryView _view;
        private CancellationToken _token;

        private Dictionary<HistoryElementModel, HistoryElementPresenter> _elements;
        
        public HistoryPresenter(HistoryModel model, HistoryView view)
        {
            _model = model;
            _view = view;
            _elements = new Dictionary<HistoryElementModel, HistoryElementPresenter>();
        }
        
        public void Setup(CancellationToken token)
        {
            _model.Elements.ObserveAdd().Subscribe(x => AddElement(x.Value)).AddTo(token);
            _model.Elements.ObserveRemove().Subscribe(x => DeleteElement(x.Value)).AddTo(token);
            _model.Elements.ObserveReset().Subscribe(_ => ClearElements()).AddTo(token);
            _token = token;
        }

        public void Cleanup()
        {
        }
        
        
        private void AddElement(HistoryElementModel elementModel)
        {
            var elementView = _view.CreateElement();
            var elementPresenter = new HistoryElementPresenter(elementModel, elementView);
            elementPresenter.Setup(_token);
            _elements.Add(elementModel, elementPresenter);
        }

        private void DeleteElement(HistoryElementModel elementModel, bool isRemove=true)
        {
            if (_elements.TryGetValue(elementModel, out var elementPresenter))
            {
                elementModel.Cleanup();
                elementPresenter.Cleanup();
                GameObject.Destroy(elementPresenter.View.gameObject);
                if (isRemove)
                {
                    _elements.Remove(elementModel);
                }
            }
        }
        
        private void ClearElements()
        {
            foreach (var element in _elements)
            {
                DeleteElement(element.Key, false);
            }
            _elements.Clear();
        }
    }
}