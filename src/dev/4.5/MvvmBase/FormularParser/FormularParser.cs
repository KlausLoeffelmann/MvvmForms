using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ActiveDevelop.MvvmBaseLib.FormularParser
{
    /// <summary>
    /// Stellt Funktionalitäten zur Verfügung, mit denen sich aus einem beliebigen mathematischen Ausdruck, 
    /// der als String vorliegt, die Ergebnisberechnung vornehmen lässt.
    /// </summary>
    /// <remarks></remarks>
    public partial class FormulaEvaluator
    {
        private string myFormular;
        private List<FormulaEvaluatorFunction> myFunctions;
        private PrioritizedOperators myPriorizedOperators;
        private static List<FormulaEvaluatorFunction> myPredefinedFunctions;
        private double myResult;
        private bool myIsCalculated;
        private List<double> myConsts;
        private int myConstEnumCounter;

        protected static double myXVariable;
        protected static double myYVariable;
        protected static double myZVariable;

        //Definiert die Standardfunktionen statisch bei der ersten Verwendung dieser Klasse.
        static FormulaEvaluator()
        {

            myPredefinedFunctions = new List<FormulaEvaluatorFunction>();

            myPredefinedFunctions.Add(new FormulaEvaluatorFunction('+', Addition, Convert.ToByte(1)));
            myPredefinedFunctions.Add(new FormulaEvaluatorFunction('-', Substraction, Convert.ToByte(1)));
            myPredefinedFunctions.Add(new FormulaEvaluatorFunction('*', Multiplication, Convert.ToByte(2)));
            myPredefinedFunctions.Add(new FormulaEvaluatorFunction('/', Division, Convert.ToByte(2)));
            myPredefinedFunctions.Add(new FormulaEvaluatorFunction('\\', Remainder, Convert.ToByte(2)));
            myPredefinedFunctions.Add(new FormulaEvaluatorFunction('^', Power, Convert.ToByte(3)));
            myPredefinedFunctions.Add(new FormulaEvaluatorFunction("PI", PI, 1));
            myPredefinedFunctions.Add(new FormulaEvaluatorFunction("Sin", Sin, 1));
            myPredefinedFunctions.Add(new FormulaEvaluatorFunction("Cos", Cos, 1));
            myPredefinedFunctions.Add(new FormulaEvaluatorFunction("Tan", Tan, 1));
            myPredefinedFunctions.Add(new FormulaEvaluatorFunction("Max", Max, -1));
            myPredefinedFunctions.Add(new FormulaEvaluatorFunction("Min", Min, -1));
            myPredefinedFunctions.Add(new FormulaEvaluatorFunction("Sqrt", Sqrt, 1));
            myPredefinedFunctions.Add(new FormulaEvaluatorFunction("Tanh", Tanh, 1));
            myPredefinedFunctions.Add(new FormulaEvaluatorFunction("LogDec", LogDec, 1));
            myPredefinedFunctions.Add(new FormulaEvaluatorFunction("XVar", XVar, 1));
            myPredefinedFunctions.Add(new FormulaEvaluatorFunction("YVar", YVar, 1));
            myPredefinedFunctions.Add(new FormulaEvaluatorFunction("ZVar", ZVar, 1));
        }

        /// <summary>
        /// Erstellt eine neue Instanz dieser Klasse.
        /// </summary>
        /// <param name="Formular">Die auszuwertende Formel, die als Zeichenkette vorliegen muss.</param>
        /// <remarks></remarks>
        public FormulaEvaluator(string Formular)
        {

            //Vordefinierte Funktionen übertragen
            myFunctions = myPredefinedFunctions;
            myFormular = Formular;
            OnAddFunctions();

        }

        //Mit dem Überschreiben dieser Funktion kann der Entwickler eigene Funktionen hinzufügen
        public virtual void OnAddFunctions()
        {
            //Nichts zu tun in der Basisversion
            return;
        }

        //Interne Funktion, die das Berechnen startet.
        private void Calculate()
        {

            string locFormular = myFormular;
            string locOpStr = "";

            //Operatorenliste anlegen
            myPriorizedOperators = new PrioritizedOperators();
            foreach (FormulaEvaluatorFunction adf in myFunctions)
            {
                if (adf.IsOperator)
                {
                    myPriorizedOperators.AddFunction(adf);
                }
            }

            //Operatoren Zeichenkette zusammenbauen

            foreach (FormulaEvaluatorFunction ops in myFunctions)
            {
                if (ops.IsOperator)
                {
                    locOpStr += "\\" + ops.FunctionName;
                }
            }

            //White-Spaces entfernen
            //Syntax-Check für Klammern
            //Negativ-Vorzeichen verarbeiten
            locFormular = PrepareFormular(locFormular, locOpStr);

            //Konstanten 'rausparsen
            locFormular = GetConsts(locFormular);

            myResult = ParseSimpleTerm(Parse(locFormular, locOpStr));
            IsCalculated = true;

        }

        //Überschreibbare Funktion, die die Formelauswertung steuert.
        protected virtual string Parse(string Formular, string OperatorRegEx)
        {

            string locTemp = null;
            Match locTerm = null;
            Match locFuncName = null;
            MatchCollection locMoreInnerTerms = null;
            List<double> locPreliminaryResult = new List<double>();
            bool locFuncFound = false;
            string locOperatorRegEx = "\\([\\d\\;" + OperatorRegEx + "]*\\)";

            FormulaEvaluatorFunction adf = null;

            locTerm = Regex.Match(Formular, locOperatorRegEx);
            if (!string.IsNullOrEmpty(locTerm.Value))
            {
                locTemp = Formular.Substring(0, locTerm.Index);

                //Befindet sich ein Funktionsname davor?
                locFuncName = Regex.Match(locTemp, "[a-zA-Z]*", RegexOptions.RightToLeft);

                //Gibt es mehrere, durch ; getrennte Parameter?
                locMoreInnerTerms = Regex.Matches(locTerm.Value, "[\\d" + OperatorRegEx + "]*[;|\\)]");

                //Jeden Parameterterm auswerten und zum Parameter-Array hinzufügen
                foreach (Match locMatch in locMoreInnerTerms)
                {
                    locTemp = locMatch.Value;
                    locTemp = locTemp.Replace(";", "").Replace(")", "");
                    locPreliminaryResult.Add(ParseSimpleTerm(locTemp));
                }

                //Möglicher Syntaxfehler: Mehrere Parameter, aber keine Funktion
                if (string.IsNullOrEmpty(locFuncName.Value) && locMoreInnerTerms.Count > 1)
                {
                    SyntaxErrorException up = new SyntaxErrorException("Fehler in Formel: Mehrere Klammerparameter aber kein Funktionsname angegeben!");
                    throw up;
                }

                if (!string.IsNullOrEmpty(locFuncName.Value))
                {
                    //Funktionsnamen suchen
                    locFuncFound = false;
                    foreach (FormulaEvaluatorFunction adfWithinLoop in myFunctions)
                    {
                        adf = adfWithinLoop;
                        if (adfWithinLoop.FunctionName.ToUpper() == locFuncName.Value.ToUpper())
                        {
                            locFuncFound = true;
                            break;
                        }
                    }

                    if (locFuncFound == false)
                    {
                        SyntaxErrorException up = new SyntaxErrorException("Fehler in Formel: Der Funktionsname wurde nicht gefunden");
                        throw up;
                    }
                    else
                    {
                        Formular = Formular.Replace(locFuncName.Value + locTerm.Value, myConstEnumCounter.ToString("000"));
                        double[] locArgs = new double[locPreliminaryResult.Count];
                        locPreliminaryResult.CopyTo(locArgs);
                        //Diese Warnung bezieht sich auf einen hypothetischen Fall,
                        //der aber nie eintreten kann! :-)
                        myConsts.Add(adf.Operate(locArgs));
                        myConstEnumCounter += 1;
                    }
                }
                else
                {
                    Formular = Formular.Replace(locTerm.Value, myConstEnumCounter.ToString("000"));
                    myConsts.Add(Convert.ToDouble(locPreliminaryResult[0]));
                    myConstEnumCounter += 1;
                }
            }
            else
            {
                return Formular;
            }
            Formular = Parse(Formular, OperatorRegEx);
            return Formular;

        }

        //Überschreibbare Funktion, die einen einfachen Term 
        //(ohne Funktionen, nur Operatoren) auswertet.
        protected virtual double ParseSimpleTerm(string Formular)
        {

            int locPos = 0;
            double locResult = 0;

            //Klammern entfernen
            if (Formular.IndexOfAny(new char[] { '(', ')' }) > -1)
            {
                Formular = Formular.Remove(0, 1);
                Formular = Formular.Remove(Formular.Length - 1, 1);
            }

            //Die Prioritäten der verschiedenen Operatoren von oben nach unten durchlaufen
            for (int locPrioCount = myPriorizedOperators.HighestPriority; locPrioCount >= myPriorizedOperators.LowestPriority; locPrioCount--)
            {
                do
                {
                    //Schauen, ob *nur* ein Wert
                    if (Formular.Length == 3)
                    {
                        return Convert.ToDouble(myConsts[int.Parse(Formular)]);
                    }

                    //Die Operatorenzeichen einer Ebene ermitteln
                    char[] locCharArray = myPriorizedOperators.OperatorChars(Convert.ToByte(locPrioCount));
                    if (locCharArray == null)
                    {
                        //Gibt keinen Operator dieser Ebene, dann n�chste Hierarchie.
                        break;
                    }

                    //Nach einem der Operatoren dieser Hierarchieebene suchen
                    locPos = Formular.IndexOfAny(locCharArray);
                    if (locPos == -1)
                    {
                        //Kein Operator dieser Ebene mehr in der Formel vorhanden - n�chste Hierarchie.
                        break;
                    }
                    else
                    {
                        double[] locDblArr = new double[2];
                        //Operator gefunden - Teilterm ausrechnen
                        locDblArr[0] = Convert.ToDouble(myConsts[int.Parse(Formular.Substring(locPos - 3, 3))]);
                        locDblArr[1] = Convert.ToDouble(myConsts[int.Parse(Formular.Substring(locPos + 1, 3))]);

                        //Die entsprechende Funktion aufrufen, die durch die Hilfsklassen
                        //anhand Priorit�t und Operatorzeichen ermittelt werden kann.
                        char locOpChar = Convert.ToChar(Formular.Substring(locPos, 1));
                        locResult = myPriorizedOperators.OperatorByChar(Convert.ToByte(locPrioCount), locOpChar).Operate(locDblArr);

                        //Und den kompletten Ausdruck durch eine neue Konstante ersetzen
                        myConsts.Add(locResult);
                        Formular = Formular.Remove(locPos - 3, 7);
                        Formular = Formular.Insert(locPos - 3, myConstEnumCounter.ToString("000"));
                        myConstEnumCounter += 1;
                    }
                } while (true);
            }
            return 0;
        }

        //{berschreibbare Funktion, die die konstanten Zahlenwerte in der Formel ermittelt.
        protected virtual string GetConsts(string formular)
        {

            Regex locRegEx = new Regex("[\\d,.]+[S]*");
            //Alle Ziffern mit Komma oder Punkt aber keine Whitespaces
            myConstEnumCounter = 0;
            myConsts = new List<double>();
            return locRegEx.Replace(formular, EnumConstsProc);

        }

        //Rückruffunktion für das Auswerten der einzelnen Konstanten (siehe vorherige Zeile).
        protected virtual string EnumConstsProc(Match m)
        {

            try
            {
                myConsts.Add(double.Parse(m.Value));
                string locString = myConstEnumCounter.ToString("000");
                myConstEnumCounter += 1;
                return locString;
            }
            catch 
            {
                myConsts.Add(double.NaN);
                return "ERR";
            }
        }

        //Hier werden vorbereitende Arbeiten durchgeführt.
        protected virtual string PrepareFormular(string Formular, string OperatorRegEx)
        {

            int locBracketCounter = 0;

            //Klammern überprüfen
            foreach (char locChar in Formular.ToCharArray())
            {
                if (locChar == '(')
                {
                    locBracketCounter += 1;
                }

                if (locChar == ')')
                {
                    locBracketCounter -= 1;
                    if (locBracketCounter < 0)
                    {
                        SyntaxErrorException up = new SyntaxErrorException("Error in Formular: Too many closing brackets.");
                        throw up;
                    }
                }
            }
            if (locBracketCounter > 0)
            {
                SyntaxErrorException up = new SyntaxErrorException("Error in Formular: An open bracket was not closed.");
                throw up;
            }

            //White-Spaces entfernen
            Formular = Regex.Replace(Formular, "\\s", "");

            //Vorzeichen verarbeiten
            if (Formular.StartsWith("-") || Formular.StartsWith("+"))
            {
                Formular = Formular.Insert(0, "0");
            }

            //Sonderfall negative Klammer
            Formular = Regex.Replace(Formular, "\\(-\\(", "(0-(");

            return Regex.Replace(Formular, "(?<operator>[" + OperatorRegEx + "\\(])-(?<zahl>[\\d\\.\\,]*)", "${operator}((0-1)*${zahl})");

        }

        /// <summary>
        /// Bestimmt oder ermittelt die zu berechnende Formel.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Formular
        {
            get
            {
                return myFormular;
            }
            set
            {
                IsCalculated = false;
                myFormular = value;
            }
        }

        /// <summary>
        /// Ermittelt die Berechnung der Formel beim ersten Aufruf; 
        /// speichert den berechneten Wert und ruft ihn bei allen folgenden Aufrufen 
        /// nur ab, sofern sich der Formeltext zwischenzeitlich nicht geändert hat.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public double Result
        {
            get
            {
                if (!IsCalculated)
                {
                    Calculate();
                }
                return myResult;
            }
        }

        /// <summary>
        /// Bestimmt oder ermittelt, ob die Formel bereits berechnet wurde. Setzen Sie diese 
        /// Eigenschaft vor der Verwendung von Result auf False, wenn Sie mit sich verändernden 
        /// Funktionen wie beispielsweise XVar arbeiten.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool IsCalculated
        {
            get
            {
                return myIsCalculated;
            }
            set
            {
                myIsCalculated = value;
            }
        }

        /// <summary>
        /// Ermittelt oder bestimmt die Funktionen, mit denen die Berechnungen 
        /// einer Formel durchgeführt wird.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public List<FormulaEvaluatorFunction> Functions
        {
            get
            {
                return myFunctions;
            }
            set
            {
                IsCalculated = false;
                myFunctions = value;
            }
        }
    }
}