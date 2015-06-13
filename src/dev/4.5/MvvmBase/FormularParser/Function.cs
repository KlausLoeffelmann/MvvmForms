using System;
using System.Collections.Generic;

namespace ActiveDevelop.MvvmBaseLib.FormularParser
{
    /// <summary>
    /// Speichert die Parameter für eine Funktion, die von der Klasse zur Berechnung beliebige mathematischer 
    /// Ausdrücke <see cref="FormulaEvaluator">FormulaEvaluator</see>berücksichtigt werden kann.
    /// </summary>
    /// <remarks></remarks>
    public class FormulaEvaluatorFunction : IComparable
    {
        /// <summary>
        /// Stellt einen Delegaten zur Verfügung, mit der benutzerdefinierte Funktionen für die Formelberechnung 
        /// durch <see cref="FormulaEvaluator">FormulaEvaluator</see> implementiert werden können.
        /// </summary>
        /// <param name="parArray"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public delegate double FormularEvaluatorFunctionDelegate(double[] parArray);

        protected string myFunctionname;
        protected int myParameters;
        protected FormularEvaluatorFunctionDelegate myFunctionProc;
        protected List<double> myConsts;
        protected bool myIsOperator;
        protected byte myPriority;

        /// <summary>
        /// Erstellt eine neue Intanz dieser Klasse. 
        /// Verwenden Sie diese Überladungsversion, um Operatoren zu erstellen, die aus einem Zeichen bestehen,
        /// </summary>
        /// <param name="OperatorChar">Das Zeichen, das den Operator darstellt.</param>
        /// <param name="FunctionProc">Der ADFunctionDelegat für die Berechnung durch diesen Operator.</param>
        /// <param name="Priority">Die Operatorpriorität (3= Potenz, 2=Punkt, 1=Strich).</param>
        /// <remarks></remarks>
        public FormulaEvaluatorFunction(char OperatorChar, FormularEvaluatorFunctionDelegate FunctionProc, byte Priority)
        {

            if (Priority < 1)
            {
                ArgumentException Up = new ArgumentException("Priority for operators cannot be smaller than 1");
                throw Up;
            }

            myFunctionname = OperatorChar.ToString();
            myParameters = 2;
            myFunctionProc = FunctionProc;
            myIsOperator = true;
            myPriority = Priority;
        }

        /// <summary>
        /// Erstellt eine neue Instanz dieser Klasse. 
        /// Verwenden Sie diese Überladungsversion, um Funktionen zu erstellen, die aus mehreren Zeichen bestehen.
        /// </summary>
        /// <param name="FunctionName">Die Zeichenfolge, die den Funktionsnamen darstellt.</param>
        /// <param name="FunctionProc">Der ADFunctionDelegat für die Berechnung durch diese Funktion.</param>
        /// <param name="Parameters">Die Anzahl der Parameter, die diese Funktion entgegen nimmt.</param>
        /// <remarks></remarks>
        public FormulaEvaluatorFunction(string FunctionName, FormularEvaluatorFunctionDelegate FunctionProc, int Parameters)
        {
            myFunctionname = FunctionName;
            myFunctionProc = FunctionProc;
            myParameters = Parameters;
            myIsOperator = false;
            myPriority = 0;
        }

        /// <summary>
        /// Liefert den Funktionsnamen bzw. das Operatorenzeichen zur�ck.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string FunctionName
        {
            get
            {
                return myFunctionname;
            }
        }

        /// <summary>
        /// Liefert die Anzahl der zur Anwendung kommenden Parameter für diese Funktion zurück.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public int Parameters
        {
            get
            {
                return myParameters;
            }
        }

        /// <summary>
        /// Zeigt an, ob es sich bei dieser Instanz um einen Operator handelt.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsOperator
        {
            get
            {
                return myIsOperator;
            }
        }

        /// <summary>
        /// Ermittelt die Priorität, die dieser Operator hat. (3= Potenz, 2=Punkt, 1=Strich)
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

        /// <summary>
        /// Ermittelt den Delegaten, der diese Funktion oder diesen Operator berechnet.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public FormularEvaluatorFunctionDelegate FunctionProc
        {
            get
            {
                return myFunctionProc;
            }
        }

        /// <summary>
        /// Ruft den Delegaten auf, der diese Funktion (diesen Operator) berechnet.
        /// </summary>
        /// <param name="parArray">Das Array, dass die Argumente der Funktion enthält.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public double Operate(double[] parArray)
        {
            if (Parameters > -1)
            {
                if (parArray.Length != Parameters)
                {
                    ArgumentException Up = new ArgumentException("Anzahl Parameter entspricht nicht der Vorschrift der Funktion " + FunctionName);
                    throw Up;
                }
            }
            return myFunctionProc(parArray);
        }

        /// <summary>
        /// Vergleicht zwei Instanzen dieser Klasse anhand ihres Prioritätswertes.
        /// </summary>
        /// <param name="obj">Eine ADFunction-Instanz, die mit dieser Instanz verglichen werden soll.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public int CompareTo(object obj)
        {
            if (obj.GetType() == typeof(FormulaEvaluatorFunction))
            {
                return myPriority.CompareTo(((FormulaEvaluatorFunction)obj).Priority) * -1;
            }
            else
            {
                ArgumentException up = new ArgumentException("Only ActiveDevelop.Function-objects can be compared or sorted.");
                throw up;
            }
        }
    }
}