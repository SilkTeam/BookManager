using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookManager.Models
{
    public class Entity
    {
        private BookManagerEntities _ef;
        public BookManagerEntities EF
        {
            get
            {
                if (_ef == null)
                    _ef = new BookManagerEntities();
                return _ef;
            }
        }
    }
}