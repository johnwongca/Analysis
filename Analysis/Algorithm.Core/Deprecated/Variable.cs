//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


//namespace Algorithm.Core
//{
    
//    public class Variable
//    {
//        protected long CountOfValueSet = -1;
//        protected double mValue = double.NaN;
        
//        public double Value { get { return mValue; } set { Set(value); } }
//        public virtual Variable Set(params double[] args)
//        {
//            CountOfValueSet++;
//            if (CountOfValueSet == 0)
//            {
//                Initialization(args);
//            }
//            Calculate(args);
//            return this;
//        }
//        protected virtual void Initialization(params double[] args)
//        {
//        }
//        protected virtual void Calculate(params double[] args)
//        {
//            mValue = args[0];
//        }
//        public override string ToString()
//        {
//            return Value.ToString();
//        }
//        #region overloaded Operators
//        public override bool Equals(object o)
//        {
//            if (o == null)
//                return false;
//            if (object.ReferenceEquals(this, o))
//                return true;
//            if (o is double)
//                return mValue == (double)o;
//            if (o is Variable)
//                return mValue == ((Variable)o).mValue;
//            return false;
//        }
//        public override int GetHashCode()
//        {
//            //if (this is AWindowOperator)
//            //    return (mValue.GetHashCode()) ^ unchecked((int)0x80000000);
//            return mValue.GetHashCode() & unchecked((int)0x7fffffff);
//        }
//        //operator ==
//        public static bool operator ==(Variable a, Variable b)
//        {
//            if (a != null)
//                return a.Equals(b);
//            return b.Equals(a);
//        }
//        //operator !=
//        public static bool operator !=(Variable a, Variable b)
//        {
//            if (a != null)
//                return !a.Equals(b);
//            return !b.Equals(a);
//        }
        
//        public static implicit operator double(Variable Variable)
//        {
//            return Variable.Value;
//        }
//        public static implicit operator Variable(double Variable)
//        {
//            return (new Variable()).Set(Variable);
//        }
//        #endregion
//    }
    
//}
