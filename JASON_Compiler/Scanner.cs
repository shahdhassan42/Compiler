using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Token_Class
{
    Read, Write, Repeat,Until, If, Elseif, Else, Then, Return, Endl,
    PlusOp, MinusOp, MultiplyOp, DivideOp,
    EqualOp, LessThanOp,GreaterThanOp, NotEqualOp,
    ANDOp, OROp,
    Int, Float, String,
    Idenifier, Constant
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
        Dictionary<string, Token_Class> Arithmatic_Operator = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Condition_Operator = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Boolean_Operator = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Datatype = new Dictionary<string, Token_Class>();
        public Scanner()
        {
            Reserved_Keywords.Add("read", Token_Class.Read);
            Reserved_Keywords.Add("write", Token_Class.Write);
            Reserved_Keywords.Add("reapeat", Token_Class.Repeat);
            Reserved_Keywords.Add("until", Token_Class.Until);
            Reserved_Keywords.Add("if", Token_Class.If);
            Reserved_Keywords.Add("elseif", Token_Class.Elseif);
            Reserved_Keywords.Add("else", Token_Class.Else);
            Reserved_Keywords.Add("then", Token_Class.Then);
            Reserved_Keywords.Add("return", Token_Class.Return);
            Reserved_Keywords.Add("endl", Token_Class.Endl);


            Arithmatic_Operator.Add("+", Token_Class.PlusOp);
            Arithmatic_Operator.Add("-", Token_Class.MinusOp);
            Arithmatic_Operator.Add("*", Token_Class.MultiplyOp);
            Arithmatic_Operator.Add("/", Token_Class.DivideOp);


            Condition_Operator.Add("=", Token_Class.EqualOp);
            Condition_Operator.Add("<", Token_Class.LessThanOp);
            Condition_Operator.Add(">", Token_Class.GreaterThanOp);
            Condition_Operator.Add("<>", Token_Class.NotEqualOp);

            Boolean_Operator.Add("&&", Token_Class.ANDOp);
            Boolean_Operator.Add("||", Token_Class.OROp);


            Datatype.Add("int", Token_Class.Int);
            Datatype.Add("float", Token_Class.Float);
            Datatype.Add("string", Token_Class.String);
        }

        public void StartScanning(string SourceCode)
        {
            for(int i=0; i<SourceCode.Length;i++)
            {
                int j = i;
                char CurrentChar = SourceCode[i];
                string CurrentLexeme = CurrentChar.ToString();

                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n')
                    continue;

                if (CurrentChar >= 'A' && CurrentChar <= 'z') //if you read a character
                {
                   
                }

                else if(CurrentChar >= '0' && CurrentChar <= '9')
                {
                   
                }
                else if(CurrentChar == '{')
                {
                   
                }
                else
                {
                   
                }
            }
            
            JASON_Compiler.TokenStream = Tokens;
        }
        void FindTokenClass(string Lex)
        {
            Token_Class TC;
            Token Tok = new Token();
            Tok.lex = Lex;
            //Is it a reserved word?
            

            //Is it an identifier?
            

            //Is it a Constant?

            //Is it an operator?

            //Is it an undefined?
        }

    

        bool isIdentifier(string lex)
        {
            bool isValid=true;
            // Check if the lex is an identifier or not.
            
            return isValid;
        }
        bool isConstant(string lex)
        {
            bool isValid = true;
            // Check if the lex is a constant (Number) or not.

            return isValid;
        }
    }
}
