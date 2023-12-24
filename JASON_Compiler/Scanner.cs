using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Token_Class
{
    Read, Write, Repeat,Until, If, Elseif, Else, Then, Return, Endl,
    PlusOp, MinusOp, MultiplyOp, DivideOp,
    EqualOp, LessThanOp,GreaterThanOp, NotEqualOp, MinusOperation,
    ANDOp, OROp,
    Dot,Comma, LParanthesis, RParanthesis,LBracket,RBracket, AssignmentOp,SemiColon,
    Int, Float, String,
    Idenifier, Number, StringLine,Main,End,
    Datatype
}
namespace JASON_Compiler
{
    

    public class Token
    {
       public string lex;
       public Token_Class token_type;
    }
    

    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        Dictionary<string, Token_Class> Reserved_Keywords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operator = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Datatype = new Dictionary<string, Token_Class>();
        public Scanner()
        {
            Reserved_Keywords.Add("read", Token_Class.Read);
            Reserved_Keywords.Add("write", Token_Class.Write);
            Reserved_Keywords.Add("repeat", Token_Class.Repeat);
            Reserved_Keywords.Add("until", Token_Class.Until);
            Reserved_Keywords.Add("if", Token_Class.If);
            Reserved_Keywords.Add("elseif", Token_Class.Elseif);
            Reserved_Keywords.Add("else", Token_Class.Else);
            Reserved_Keywords.Add("then", Token_Class.Then);
            Reserved_Keywords.Add("return", Token_Class.Return);
            Reserved_Keywords.Add("endl", Token_Class.Endl);
            Reserved_Keywords.Add("main", Token_Class.Main);
            Reserved_Keywords.Add("end", Token_Class.End);

            Operator.Add("+", Token_Class.PlusOp);
            Operator.Add("-", Token_Class.MinusOp);
            Operator.Add("*", Token_Class.MultiplyOp);
            Operator.Add("/", Token_Class.DivideOp);
           // Operator.Add("–", Token_Class.MinusOperation);

            Operator.Add("=", Token_Class.EqualOp);
            Operator.Add("<", Token_Class.LessThanOp);
            Operator.Add(">", Token_Class.GreaterThanOp);
            Operator.Add("<>", Token_Class.NotEqualOp);

            Operator.Add("&&", Token_Class.ANDOp);
            Operator.Add("||", Token_Class.OROp);

            Operator.Add(".", Token_Class.Dot);
            Operator.Add(",", Token_Class.Comma);
            Operator.Add("(", Token_Class.LBracket);
            Operator.Add(")", Token_Class.RBracket);
            Operator.Add("{", Token_Class.LParanthesis);
            Operator.Add("}", Token_Class.RParanthesis);
            Operator.Add(":=", Token_Class.AssignmentOp);
            Operator.Add(";", Token_Class.SemiColon);

            Datatype.Add("int", Token_Class.Int);
            Datatype.Add("float", Token_Class.Float);
            Datatype.Add("string", Token_Class.String);
        }

        public void StartScanning(string SourceCode)
        {
            for (int i = 0; i < SourceCode.Length; i++)
            {
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();

                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n')
                    continue;
                //identifier or fun name
                if (CurrentChar >= 'A' && CurrentChar <= 'z') //if you read a character
                {
                    
                        while (true)
                    {

                        j++;
                        if (j == SourceCode.Length) { 
                            i = j; 
                            break;
                        }
                        if (CurrentChar == '_')
                        {
                            CurrentLexeme += CurrentChar.ToString();
                            FindTokenClass(CurrentLexeme);
                            CurrentLexeme = "";
                        }
                        CurrentChar = SourceCode[j];
                        if ((CurrentChar >= 'A' && CurrentChar <= 'z') || 
                            (CurrentChar >= '0' && CurrentChar <= '9'))
                        {
                            CurrentLexeme += CurrentChar.ToString();
                        }
                        else
                        {
                            i = j - 1; break;
                        }
                    }
                    FindTokenClass(CurrentLexeme);
                }

                else if(CurrentChar >= '0' && CurrentChar <= '9')
                {
                    while (true)
                    {
                        j++;
                        if (j == SourceCode.Length) {
                            i = j;
                            break;
                        }
                        CurrentChar = SourceCode[j];

                        if (CurrentChar == ' ' || CurrentChar == '+' || CurrentChar == ':' || CurrentChar == '=' ||
                            CurrentChar == '>' || CurrentChar == '<' || CurrentChar == '*' || CurrentChar == '-' ||
                            CurrentChar == '/' || CurrentChar == ';' || CurrentChar == ')' || CurrentChar == ',' || CurrentChar == '\n' || CurrentChar == '\r')
                        {
                            i = j - 1; break;
                        }
                        CurrentLexeme += CurrentChar.ToString();
                    }
                    FindTokenClass(CurrentLexeme);
                }
                //comment
                else if (CurrentChar == '/' && j + 1 < SourceCode.Length && SourceCode[j + 1] == '*')
                {

                    while ((CurrentChar != '*' || SourceCode[j + 1] != '/'))
                    {
                        j++;
                        CurrentChar = SourceCode[j];
                        CurrentLexeme += CurrentChar.ToString();

                        if (j == SourceCode.Length - 1)
                        {
                            FindTokenClass(CurrentLexeme);
                            break;
                        }
                    }
                    i = j + 1;
                }
                //string
                // string
                else if (CurrentChar == '"')
                {
                    j++;
                    while (j < SourceCode.Length && j != '\n')
                    {
                        CurrentLexeme += SourceCode[j].ToString();
                        j++;
                        if (SourceCode[j - 1] == '"')
                        {
                            break;
                        }
                    }
                    FindTokenClass(CurrentLexeme.Trim());
                    i = j - 1;
                    continue;
                }

                //boolean &&
                else if (CurrentChar == '&')
                {
                    bool ok = true;
                    j++;
                    if (j == SourceCode.Length) { 
                        FindTokenClass(CurrentLexeme);
                         i = j;
                        ok = false;
                    }
                    if (ok)
                    {
                        CurrentChar = SourceCode[j];
                        if (CurrentChar == '&')
                        {
                            CurrentLexeme += CurrentChar.ToString();
                        }
                        FindTokenClass(CurrentLexeme);
                        i = j;
                    }
                }
                //condition op <>
                else if (CurrentChar == '<') 
                {
                    bool ok = true;
                    j++;
                    if (j == SourceCode.Length) { FindTokenClass(CurrentLexeme); i = j; ok = false; }
                    if (ok)
                    {
                        CurrentChar = SourceCode[j];

                           if (CurrentChar == '>')
                        {
                            CurrentLexeme += CurrentChar.ToString();
                        }
                           else if (CurrentChar == '=')
                        {
                            CurrentLexeme += CurrentChar.ToString();
                        }
                            FindTokenClass(CurrentLexeme);
                        i = j;
                    }
                }
                else if (CurrentChar == '>')
                {
                    bool ok = true;
                    j++;
                    if (j == SourceCode.Length) { FindTokenClass(CurrentLexeme); i = j; ok = false; }
                    if (ok)
                    {
                        CurrentChar = SourceCode[j];

                      if (CurrentChar == '=')
                        {
                            CurrentLexeme += CurrentChar.ToString();
                        }
                        FindTokenClass(CurrentLexeme);
                        i = j;
                    }
                }


                else if (CurrentChar == '.')
                {
                    while (true)
                    {
                        j++;
                        if (j == SourceCode.Length) { i = j; break; }

                        CurrentChar = SourceCode[j];
                        if ((CurrentChar >= '0' && CurrentChar <= '9'))
                        {
                            CurrentLexeme += CurrentChar.ToString();
                        }
                        else
                        {
                            i = j - 1; break;
                        }
                    }
                    FindTokenClass(CurrentLexeme);
                }
                // ASSIGNMENT_OPERATOR [:=]
                else if (CurrentChar == ':')
                {
                    bool ok = true;
                    j++;
                    if (j == SourceCode.Length) { i = j; ok = false; }
                    if (ok)
                    {
                        CurrentChar = SourceCode[j];
                        if (CurrentChar == '=')
                        {
                            CurrentLexeme += CurrentChar.ToString();
                        }
                        FindTokenClass(CurrentLexeme);
                        i = j;
                    }
                }
                else if (CurrentChar == '|')
                {
                    bool ok = true;
                    j++;
                    if (j == SourceCode.Length) { i = j; ok = false; }
                    if (ok)
                    {
                        CurrentChar = SourceCode[j];
                        if (CurrentChar == '|')
                        {
                            CurrentLexeme += CurrentChar.ToString();
                        }
                        FindTokenClass(CurrentLexeme);
                        i = j;
                    }
                }
               else
                {
                    // check for [Error, Arithmatic_Operator, Condition_Operator ]
                    FindTokenClass(CurrentLexeme);
                }
            }
            
            TINY_Compiler.TokenStream = Tokens;
        }
        void FindTokenClass(string Lex)
        {
            Token_Class TC;
            Token Tok = new Token();
            Tok.lex = Lex;
        
            //Is it a reserved word?
            if (Reserved_Keywords.ContainsKey(Lex))
            {
                // [key][value]
                Tok.token_type = Reserved_Keywords[Lex];
                Tokens.Add(Tok);
            }

            // Is it a datatype?
            else if (Datatype.ContainsKey(Lex))
            {
                Tok.token_type = Datatype[Lex];
                Tokens.Add(Tok);
            }
            //Is it an operator?
            else if (Operator.ContainsKey(Lex))
            {
                Tok.token_type = Operator[Lex];
                Tokens.Add(Tok);
            }
            //Is it an identifier?

            else if (isIdentifier(Lex))
            {
                // Search in REGEX
                Tok.token_type = Token_Class.Idenifier;
                Tokens.Add(Tok);
            }
            else if (isNumber(Lex))
            {
                // Search in REGEX
                Tok.token_type = Token_Class.Number;
                Tokens.Add(Tok);
            }
           
            else if (isString(Lex))
            {
                Tok.token_type = Token_Class.StringLine;
                Tokens.Add(Tok);
            }
            //Is it an undefined?
            else
            {
                Errors.Error_List.Add(Lex);
            }
        }



        bool isIdentifier(string lex)
        {
            var regexId = new Regex(@"^([a-z]|[A-Z])([a-z]|[A-Z]|[0-9])*$", RegexOptions.Compiled);
            bool isValid = regexId.IsMatch(lex);
            return isValid;
        }
        bool isNumber(string lex)
        {
            var regexNumber = new Regex(@"^[0-9]+(\.[0-9]+)?$", RegexOptions.Compiled);
            
            bool isValid = regexNumber.IsMatch(lex);
            return isValid;
        }
        bool isString(string lex)
        {
            var regexString = new Regex("\"[^\n]*\"", RegexOptions.Compiled);
            bool isValid = regexString.IsMatch(lex);
            return isValid;

        }

    }
}
