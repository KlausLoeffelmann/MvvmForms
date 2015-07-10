using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ActiveDevelop.MvvmBaseLib.FormulaEvaluator
{
    /// <summary>
    /// Auflistung, in der alle Operatoren gleicher Priorität gesammelt werden, damit 
    /// es die Möglichkeit gibt, sie von links nach rechts (in einem Rutsch) zu verarbeiten.
    /// </summary>
    /// <remarks></remarks>
    public class ADOperatorsOfSamePriority : Collection<FormulaEvaluatorFunction>
    {
        private byte myPriority;

        public ADOperatorsOfSamePriority() : base()
        {
        }

        protected override void InsertItem(int index, FormulaEvaluatorFunction item)
        {
            if (!item.IsOperator)
            {
                ArgumentException locEx = new ArgumentException("Only Operators (no functions) can be added to this collection!");
                throw locEx;
            }
            if (this.Count == 0)
            {
                myPriority = item.Priority;
            }
            else
            {
                //Check, if same priority, otherwise --> Exception.
                if (item.Priority != myPriority)
                {
                    ArgumentException locEx = new ArgumentException("Only operators of priority " + myPriority + " can be added to this collection!");
                    throw locEx;
                }
            }
            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, FormulaEvaluatorFunction item)
        {
            ArgumentException locEx = new ArgumentException("This collection is supposed to be immutable!");
            throw locEx;
        }

        /// <summary>
        /// Returns priority of operator collection.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte Priority
        {
            get
            {
                return myPriority;
            }
        }
    }

    /// <summary>
    /// Groups all operatorlists by priority into a parent collection.
    /// </summary>
    /// <remarks></remarks>
    public class PrioritizedOperators : KeyedCollection<byte, ADOperatorsOfSamePriority>
    {
        private byte myHighestPriority;
        private byte myLowestPriority;

        /// <summary>
        /// Fügt einer der untergeordneten Auflistungen einen neuen Operator hinzu, 
        /// in Abhängigkeit von seiner Priorität.
        /// </summary>
        /// <param name="Function"></param>
        /// <remarks></remarks>
        public void AddFunction(FormulaEvaluatorFunction Function)
        {
            //Feststellen, ob es schon eine Auflistung für diese Operator-Priorität gibt
            if (this.Contains(Function.Priority))
            {
                //Ja - dieser hinzufügen,
                this[Function.Priority].Add(Function);
            }
            else
            {
                //Nein - anlegen und hinzufügen.
                ADOperatorsOfSamePriority locOperatorsOfSamePriority = new ADOperatorsOfSamePriority();
                locOperatorsOfSamePriority.Add(Function);
                this.Add(locOperatorsOfSamePriority);
            }
        }

        protected override byte GetKeyForItem(ADOperatorsOfSamePriority item)
        {
            return item.Priority;
        }

        protected override void InsertItem(int index, ADOperatorsOfSamePriority item)
        {

            if (this.Count == 0)
            {
                myHighestPriority = item.Priority;
                myLowestPriority = item.Priority;
                base.InsertItem(index, item);
                return;
            }

            base.InsertItem(index, item);

            if (myHighestPriority < item.Priority)
            {
                myHighestPriority = item.Priority;
            }

            if (myLowestPriority > item.Priority)
            {
                myLowestPriority = item.Priority;
            }
        }

        /// <summary>
        /// Liefert alle Operatorzeichen einer bestimmten Priorität als Char-Array zurück.
        /// </summary>
        /// <param name="Priority">Die Priorität, deren Operatoren zusammengestellt werden sollen.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public char[] OperatorChars(byte Priority)
        {
            if (this.Contains(Priority))
            {
                List<char> locChars = new List<char>();
                foreach (FormulaEvaluatorFunction locFunction in this[Priority])
                {
                    locChars.Add(Convert.ToChar(locFunction.FunctionName));
                }
                return locChars.ToArray();
            }
            return null;
        }

        /// <summary>
        /// Liefert die Funktion zurück, die sich durch ein Operator-Zeichen einer bestimmten Priorität ergibt.
        /// </summary>
        /// <param name="Priority">Die Priorität, die den Operatoren entspricht, die nach den Operatorzeichen durchsucht werden sollen.</param>
        /// <param name="OperatorChar">Das Operatorzeichen mit der angegebenen Priorität, dessen Funktionsklasse ermittelt werden soll.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public FormulaEvaluatorFunction OperatorByChar(byte Priority, char OperatorChar)
        {
            if (this.Contains(Priority))
            {
                foreach (FormulaEvaluatorFunction locFunction in this[Priority])
                {
                    if (OperatorChar == Convert.ToChar(locFunction.FunctionName))
                    {
                        return locFunction;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Liefert die höchste Prioritätennummer zurück.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte HighestPriority
        {
            get
            {
                return myHighestPriority;
            }
        }

        /// <summary>
        /// Liefert die kleinste Prioritätennummer zurück.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public byte LowestPriority
        {
            get
            {
                return myLowestPriority;
            }
        }
    }

    public class SyntaxErrorException : Exception
    {
        public SyntaxErrorException(string message) : base(message) { }
        public SyntaxErrorException(string message, Exception innerException) : base(message, innerException) { }
    }
}