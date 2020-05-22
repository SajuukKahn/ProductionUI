using ProductionUI.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace ProductionUI.ViewModels
{
    public class ProductionCardDeckViewModel : IList<ProductionCardModel>
    {
        private readonly IList<ProductionCardModel> _list = new List<ProductionCardModel>();

        #region Implementation of IEnumerable

        public IEnumerator<ProductionCardModel> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<T>

        public void Add(ProductionCardModel item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(ProductionCardModel item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(ProductionCardModel[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public bool Remove(ProductionCardModel item)
        {
            return _list.Remove(item);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsReadOnly
        {
            get { return _list.IsReadOnly; }
        }

        #endregion

        #region Implementation of IList<T>

        public int IndexOf(ProductionCardModel item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, ProductionCardModel item)
        {
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public ProductionCardModel this[int index]
        {
            get { return _list[index]; }
            set { _list[index] = value; }
        }

        #endregion

        #region Your Added Stuff

        // Add new features to your collection.

        #endregion
    }
}

