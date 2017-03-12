// Copyright (c) 2016-2017 meil
//
// This file is part of Twichirp.
// 
// Twichirp is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Twichirp is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with Twichirp.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Twichirp.Core.App.Model {
    public class BaseModel : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;

        protected ITwichirpApplication Application { get; }

        public BaseModel(ITwichirpApplication application) {
            Application = application;
        }

        protected void RaisePropertyChanged(string name) {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(name));
        }

        protected bool SetValue<T>(ref T backingField,T value,string propertyName = null) {
            if(Equals(backingField,value)) {
                return false;
            }
            backingField = value;
            if(propertyName != null) {
                RaisePropertyChanged(propertyName);
            }
            return true;
        }

        // とても遅い
        protected bool SetProperty<TTarget, TValue>(TTarget target,Expression<Func<TTarget,TValue>> outExpression,TValue value) {
            if(outExpression.Body is MemberExpression == false) {
                return false;
            }
            var expression = outExpression.Body as MemberExpression;
            if(expression.Member is PropertyInfo == false) {
                return false;
            }
            var property = expression.Member as PropertyInfo;
            if(Equals(property.GetValue(target),value)) {
                return false;
            }
            property.SetValue(target,value);
            RaisePropertyChanged(property.Name);
            return true;
        }
    }
}
