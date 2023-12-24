using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JASON_Compiler
{

    public class Node
    {
        public List<Node> Children = new List<Node>();

        public string Name;
        public Node(string N)
        {
            this.Name = N;
        }
    }



    public class Parser
    {
        int InputPointer = 0;
        List<Token> TokenStream;
        public Node root;

        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
            Flag.out_of_count = false;
            this.TokenStream = TokenStream;
            // starting sympol
            root = new Node("Program");
            root.Children.Add(Program());
            return root;
        }


        public class Flag
        {
            public static bool out_of_count = false;
            public static bool found_return = false;
        }


        //done
        Node Program()
        {
            // completed
            // Program → Function_statements Main_function

            if (Flag.out_of_count == true) { return null; }

            Node program = new Node("Program");
            //if (TokenStream[InputPointer].token_type== Token_Class.Int && TokenStream[InputPointer+1].token_type == Token_Class.Main)
            //{
                program.Children.Add(Main_function());
                program.Children.Add(Program_dash());
            //}
            //else 
            //{
            //    program.Children.Add(Program_dash());
            //    program.Children.Add(Main_function());
            //}
            return program;
        }

        //done
        Node Program_dash()
        {
            // completed
            // Program → Function_statements Main_function

            if (Flag.out_of_count == true) { return null; }


            Node program_dash = new Node("Program_dash");
            if (TokenStream[InputPointer].token_type == Token_Class.Float || TokenStream[InputPointer].token_type == Token_Class.Int ||
                TokenStream[InputPointer].token_type == Token_Class.String)
            {
                program_dash.Children.Add(Function_statement());

                program_dash.Children.Add(Program_dash());
            }
            else
            {
                return null;
            }
            return program_dash;
        }

        //done
        Node Function_statement()
        {
            // completed
            // Function_statement → Function_declaration Function_body | 𝜀

            if (Flag.out_of_count == true) { return null; }
            Node function_statement = new Node("Function_statement");
            function_statement.Children.Add(Function_declaration());
            function_statement.Children.Add(Function_body());
            return function_statement;
        }

        //done
        Node Function_declaration()
        {
            // completed
            // Function _declaration → Datatype identifier (Parameters)

            if (Flag.out_of_count == true) { return null; }
            Node function_declaration = new Node("Function_declaration");
            function_declaration.Children.Add(Datatype());
            function_declaration.Children.Add(match(Token_Class.Idenifier));
            function_declaration.Children.Add(match(Token_Class.LBracket));
            if (TokenStream[InputPointer].token_type == Token_Class.String || TokenStream[InputPointer].token_type == Token_Class.Int
                || TokenStream[InputPointer].token_type == Token_Class.Float)
            {
                function_declaration.Children.Add(Parameter_List());
            }
            function_declaration.Children.Add(match(Token_Class.RBracket));
            return function_declaration;
        }

        //done
        Node Function_body()
        {
            // completed
            // Function _body → { Statements Return_statement }

            if (Flag.out_of_count == true) { return null; }

            Node function_body = new Node("Function_body");
            function_body.Children.Add(match(Token_Class.LParanthesis));
            function_body.Children.Add(Statement_List());
            if(Flag.found_return==true)
            {
                function_body.Children.Add(match(Token_Class.RParanthesis));
            }
            else if(Flag.found_return==false)
            {
                function_body.Children.Add(Return_statement());
                function_body.Children.Add(match(Token_Class.RParanthesis));
            }
            Flag.found_return = false;
            return function_body;
        }

        //done
        Node Main_function()
        {
            // completed
            // Main_function → Datatype main() Function_body

            if (Flag.out_of_count == true)
            {
                match(Token_Class.Int);
                match(Token_Class.Main);
                return null;
            }


            Node main_function = new Node("Main_function");
            main_function.Children.Add(match(Token_Class.Int));
            main_function.Children.Add(match(Token_Class.Main));
            main_function.Children.Add(match(Token_Class.LBracket));
            main_function.Children.Add(match(Token_Class.RBracket));
            main_function.Children.Add(Function_body());
            return main_function;
        }

        Node Datatype()
        {
            // Completed
            // Datatype → int | float | string

            if (Flag.out_of_count == true) { return null; }


            Node datatype = new Node("Datatype");
            if (TokenStream[InputPointer].token_type == Token_Class.Int || TokenStream[InputPointer].token_type == Token_Class.Float || TokenStream[InputPointer].token_type == Token_Class.String)
            {
                datatype.Children.Add(match(TokenStream[InputPointer].token_type));
            }
            else
            {
                datatype.Children.Add(match(Token_Class.Datatype));
            }

            return datatype;
        }

        //done
        Node Arithmatic_Operator()
        {
            Node arithmatic_Operator = new Node("Arithmatic_Operator");

            if (Flag.out_of_count == true) { return null; }

            
            if (TokenStream[InputPointer].token_type == Token_Class.PlusOp || TokenStream[InputPointer].token_type == Token_Class.MinusOp ||
                TokenStream[InputPointer].token_type == Token_Class.DivideOp || TokenStream[InputPointer].token_type == Token_Class.MultiplyOp)
            {
                arithmatic_Operator.Children.Add(match(TokenStream[InputPointer].token_type));
            }
            return arithmatic_Operator;
        }

        //done
        Node Statement_List()
        {
            // completed
            // Statements → Statement Statements’

            if (Flag.out_of_count == true) { return null; }


            Node statement_List = new Node("Statement_List");
            statement_List.Children.Add(Statement());
            statement_List.Children.Add(Statement_List_dash());
            return statement_List;
        }

        //done
        Node Statement()
        {
            // Statement → Write_statement | Read_statement | Assignment_statement | Declaration_statement | If_statement |Repeat_statement | Function_call
            if (Flag.out_of_count == true) { return null; }

            Node statement = new Node("Statement");
            if (TokenStream[InputPointer].token_type == Token_Class.Write)
            {
                statement.Children.Add(Write_statement());
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Read)
            {
                statement.Children.Add(Read_statement());
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Idenifier && TokenStream[InputPointer + 1].token_type == Token_Class.AssignmentOp)
            {
                statement.Children.Add(Assignment_statement());
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Int || TokenStream[InputPointer].token_type == Token_Class.Float || TokenStream[InputPointer].token_type == Token_Class.String)
            {
                statement.Children.Add(Declaration_statement());
                Console.WriteLine("in declaration");
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Return)
            {
                Flag.found_return = true;
                statement.Children.Add(Return_statement());
                return statement;
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.If)
            {
                statement.Children.Add(If_statement());
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Repeat)
            {
                statement.Children.Add(Repeat_statement());
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Idenifier && TokenStream[InputPointer + 1].token_type == Token_Class.LBracket) 
            {
                statement.Children.Add(Function_call());

            }

            return statement;
        }

        ///done
        Node Statement_List_dash()
        {
            // Statements’ → Statement Statements’ | 𝜀
            if (Flag.out_of_count == true) { return null; }

            Node statement_List_dash = new Node("Statement_List_dash");
            if (InputPointer +1< TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.Read || TokenStream[InputPointer].token_type == Token_Class.Write ||
               TokenStream[InputPointer + 1].token_type == Token_Class.AssignmentOp || TokenStream[InputPointer].token_type == Token_Class.String || 
               TokenStream[InputPointer].token_type == Token_Class.Int || TokenStream[InputPointer].token_type == Token_Class.Float ||
               TokenStream[InputPointer].token_type == Token_Class.If || TokenStream[InputPointer].token_type == Token_Class.Repeat ||
               TokenStream[InputPointer + 1].token_type == Token_Class.LBracket || TokenStream[InputPointer].token_type == Token_Class.Return)
                {
                    statement_List_dash.Children.Add(Statement());
                    statement_List_dash.Children.Add(Statement_List_dash());

                    return statement_List_dash;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        //done
        Node Return_statement()
        {
            // completed    
            // Return_statement → return Expression ;

            if (Flag.out_of_count == true)
            {
                match(Token_Class.Return);
                return null;
            }
            Node return_statement = new Node("Return_statement");
            return_statement.Children.Add(match(Token_Class.Return));
            return_statement.Children.Add(Expression());
            return_statement.Children.Add(match(Token_Class.SemiColon));
            return return_statement;
        }

        //done
        Node Write_statement()
        {
            // completed    
            // Write_statement → write Write_statement_Dash


            if (Flag.out_of_count == true) { return null; }


            Node write_statement = new Node("Write_statement");
            write_statement.Children.Add(match(Token_Class.Write));
            if (TokenStream[InputPointer].token_type == Token_Class.Endl)
            {
                write_statement.Children.Add(match(Token_Class.Endl));
                write_statement.Children.Add(match(Token_Class.SemiColon));
            }
            else
            {
                write_statement.Children.Add(Expression());
                write_statement.Children.Add(match(Token_Class.SemiColon));
            }

            return write_statement;
        }

        //done
        Node Read_statement()
        {
            // completed
            // Read_statement → read identifier ;


            if (Flag.out_of_count == true) { return null; }


            Node read_statement = new Node("Read_statement");
            read_statement.Children.Add(match(Token_Class.Read));
            read_statement.Children.Add(match(Token_Class.Idenifier));
            read_statement.Children.Add(match(Token_Class.SemiColon));
            return read_statement;
        }

        //done
        Node Expression()
        {
            // Expression → stringLine | Term | Equation

            if (Flag.out_of_count == true) { return null; }

            Node expression = new Node("Expression");

            if (TokenStream[InputPointer].token_type == Token_Class.StringLine)
            {
                expression.Children.Add(match(Token_Class.StringLine));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.LBracket && (TokenStream[InputPointer+1].token_type == Token_Class.LBracket ||
                (TokenStream[InputPointer+1].token_type == Token_Class.Number || (TokenStream[InputPointer+1].token_type == Token_Class.Idenifier && TokenStream[InputPointer+1 + 1].token_type == Token_Class.LParanthesis) 
                ||TokenStream[InputPointer+1].token_type == Token_Class.Idenifier)))
            {
                expression.Children.Add(match(Token_Class.LBracket));
                expression.Children.Add(Equation());
                expression.Children.Add(match(Token_Class.RBracket));
                expression.Children.Add(Equation_Dash());
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Number || (TokenStream[InputPointer].token_type == Token_Class.Idenifier && TokenStream[InputPointer + 1].token_type == Token_Class.LParanthesis) ||
                TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                expression.Children.Add(Term());
                expression.Children.Add(Expression_dash());
            }

            return expression;
        }

        //done
        Node Expression_dash()
        {
           
            if (Flag.out_of_count == true) { return null; }

            Node expression_dash = new Node("Expression_dash");

            if (TokenStream[InputPointer].token_type == Token_Class.PlusOp || TokenStream[InputPointer].token_type == Token_Class.MinusOp || TokenStream[InputPointer].token_type == Token_Class.DivideOp || TokenStream[InputPointer].token_type == Token_Class.MultiplyOp)
            {
                expression_dash.Children.Add(Equation_Dash());
            }
            else
            {
                return null;
            }

            return expression_dash;
        }

        //done
        Node Term()
        {
            // Term → number | identifier | Function_
            if (Flag.out_of_count == true) { return null; }
            Node term = new Node("Term");
            if (TokenStream[InputPointer].token_type == Token_Class.Number)
            {
                term.Children.Add(match(Token_Class.Number));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.Idenifier &&
                TokenStream[InputPointer+3].token_type != Token_Class.Comma && TokenStream[InputPointer + 2].token_type != Token_Class.RBracket)
            {
                term.Children.Add(match(Token_Class.Idenifier));
                term.Children.Add(Term_dash());
            }
            else if(TokenStream[InputPointer].token_type == Token_Class.Idenifier && 
                (TokenStream[InputPointer + 2].token_type == Token_Class.RBracket || TokenStream[InputPointer + 3].token_type == Token_Class.Comma))
            {
                term.Children.Add(Function_call());
            }
            return term;
        }
        //done
        Node Term_dash()
        {//z1 := z1 + sum (1,a)
            // Term → number | identifier | Function_call
            if (Flag.out_of_count == true) { return null; }
            Node term_dash = new Node("Term_dash");
            if(TokenStream[InputPointer].token_type == Token_Class.LBracket)
            {
                term_dash.Children.Add(match(Token_Class.LBracket));
                if (TokenStream[InputPointer].token_type == Token_Class.Idenifier)
                {
                    term_dash.Children.Add(Identifier_list());
                }
                term_dash.Children.Add(match(Token_Class.RBracket));
            }
            else
            {
                return null;
            }
            return term_dash;
        }

        //done
        Node Function_call()
        {
            // completed
            // Function_call → identifier (Identifier_list | 𝜀) 

            if (Flag.out_of_count == true) { return null; }

            Node function_call = new Node("Function_call");
            function_call.Children.Add(match(Token_Class.Idenifier));
            function_call.Children.Add(match(Token_Class.LBracket));
            if (TokenStream[InputPointer].token_type == Token_Class.Number || TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                function_call.Children.Add(Argument_list());
                function_call.Children.Add(match(Token_Class.RBracket));
            }
            else
            {
                function_call.Children.Add(match(Token_Class.RBracket));
                return function_call;
            }
          
            return function_call;
        }

        Node Argument()
        {
            if (Flag.out_of_count == true) { return null; }
            Node argument = new Node("Argument");
            if(TokenStream[InputPointer].token_type == Token_Class.Number)
            {
                argument.Children.Add(match(Token_Class.Number));
            }
            else if(TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                argument.Children.Add(match(Token_Class.Idenifier));
            }
            return argument;
        }
        Node Argument_list()
        {
            if (Flag.out_of_count == true) { return null; }
            Node argument_list = new Node("Argument_list");
            argument_list.Children.Add(Argument());
            argument_list.Children.Add(Argument_list_dash());
            return argument_list;
        }
        Node Argument_list_dash()
        {
            if (Flag.out_of_count == true) { return null; }
            Node argument_list_dash = new Node("Argument_list_dash");
            if (TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                argument_list_dash.Children.Add(match(Token_Class.Comma));
                argument_list_dash.Children.Add(Argument());
                argument_list_dash.Children.Add(Argument_list_dash());
                return argument_list_dash;
            }
            else
            {
                return null;
            }
           
        }
        //done
        Node Identifier_list()
        {
            // completed
            // Identifier_list → Id Identifier_list_Dash


            if (Flag.out_of_count == true) { return null; }
            Node identifier_list = new Node("Identifier_list");
            identifier_list.Children.Add(match(Token_Class.Idenifier));
            identifier_list.Children.Add(Identifier_list_Dash());
            return identifier_list;
        }

        //done
        Node Identifier_list_Dash()
        {
            // completed
            // Identifier_list’ → , Id Identifier_list’ | eplson


            if (Flag.out_of_count == true) { return null; }


            Node identifier_list_Dash = new Node("Identifier_list_Dash");
            if (TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                if (TokenStream[InputPointer+1].token_type == Token_Class.Idenifier && TokenStream[InputPointer + 2].token_type != Token_Class.AssignmentOp)
                {
                    identifier_list_Dash.Children.Add(match(Token_Class.Comma));
                    identifier_list_Dash.Children.Add(match(Token_Class.Idenifier));
                    identifier_list_Dash.Children.Add(Identifier_list_Dash());
                    return identifier_list_Dash;
                }
                else if(TokenStream[InputPointer+1].token_type == Token_Class.Idenifier && TokenStream[InputPointer + 2].token_type == Token_Class.AssignmentOp)
                {
                    return null;
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        Node ConditionOp()
        {
            // completed
            // ConditionOp → notEqualOp | equalOp | lessThanOp |greaterThanOp


            if (Flag.out_of_count == true) { return null; }



            Node conditionOp = new Node("ConditionOp");
            if (TokenStream[InputPointer].token_type == Token_Class.NotEqualOp)
            {
                conditionOp.Children.Add(match(Token_Class.NotEqualOp));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.EqualOp)
            {
                conditionOp.Children.Add(match(Token_Class.EqualOp));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.LessThanOp)
            {
                conditionOp.Children.Add(match(Token_Class.LessThanOp));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.GreaterThanOp)
            {
                conditionOp.Children.Add(match(Token_Class.GreaterThanOp));
            }
            return conditionOp;
        }

        //done
        Node BooleanOp()
        {
            // completed
            //  BooleanOp → andOp | orOp


            if (Flag.out_of_count == true) { return null; }



            Node booleanOp = new Node("BooleanOp");
            if (TokenStream[InputPointer].token_type == Token_Class.ANDOp || TokenStream[InputPointer].token_type == Token_Class.OROp)
            {
                booleanOp.Children.Add(match(TokenStream[InputPointer].token_type));
            }
            return booleanOp;
        }

        //done
        Node Equation()
        {
            if (Flag.out_of_count == true) { return null; }

            Node equation = new Node("Equation");

            if (TokenStream[InputPointer].token_type == Token_Class.LBracket)
            {
                equation.Children.Add(match(Token_Class.LBracket));
                equation.Children.Add(Equation());

                equation.Children.Add(match(Token_Class.RBracket));
                equation.Children.Add(Equation_Dash());
            }
            else
            {
                equation.Children.Add(Term());
                equation.Children.Add(Equation_Dash());
            }

            return equation;
        }

        //done
        Node Equation_Dash()
        {

            if (Flag.out_of_count == true) { return null; }


            Node equation_dash = new Node("Equation_Dash");

            if (TokenStream[InputPointer].token_type == Token_Class.PlusOp || TokenStream[InputPointer].token_type == Token_Class.MinusOp || TokenStream[InputPointer].token_type == Token_Class.DivideOp || TokenStream[InputPointer].token_type == Token_Class.MultiplyOp)
            {
                equation_dash.Children.Add(Arithmatic_Operator());
                equation_dash.Children.Add(Equation());
                equation_dash.Children.Add(Equation_Dash());
                return equation_dash;
            }
            else
            {
                return null;
            }
        }

        //done
        Node Condition()
        {
            // completed
            // Condition → identifier ConditionOp Term

            if (Flag.out_of_count == true) { return null; }

            Node condition = new Node("Condition");
            condition.Children.Add(match(Token_Class.Idenifier));
            condition.Children.Add(ConditionOp());
            condition.Children.Add(Term());
            return condition;
        }

        //done
        Node Condition_statement()
        {
            // completed
            // Condition_statement → Condition Condition_statement_Dash

            if (Flag.out_of_count == true) { return null; }

            Node condition_statement = new Node("Condition_statement");
            condition_statement.Children.Add(Condition());
            condition_statement.Children.Add(Condition_statement_Dash());
            return condition_statement;
        }

        //done
        Node Condition_statement_Dash()
        {
            if (Flag.out_of_count == true) { return null; }

            Node condition_statement_Dash = new Node("Condition_statement_Dash");

            if (TokenStream[InputPointer].token_type == Token_Class.ANDOp || TokenStream[InputPointer].token_type == Token_Class.OROp)
            {
                condition_statement_Dash.Children.Add(BooleanOp());
                condition_statement_Dash.Children.Add(Condition());
                Node remainingConditions = Condition_statement_Dash();
                if (remainingConditions != null)
                {
                    condition_statement_Dash.Children.Add(remainingConditions);
                }
            }

            return condition_statement_Dash.Children.Count > 0 ? condition_statement_Dash : null;
        }
        // ============================================== Gaber end =======================================================


        // ============================================== Nour start =======================================================

        //done
        Node Assignment_statement()
        {
            // completed
            // Assignment_statement → identifier assignmentOp Expression ;


            if (Flag.out_of_count == true) { return null; }


            Node assignment_statement = new Node("Assignment_statement");

            assignment_statement.Children.Add(match(Token_Class.Idenifier));
            assignment_statement.Children.Add(match(Token_Class.AssignmentOp));
            assignment_statement.Children.Add(Expression());
            assignment_statement.Children.Add(match(Token_Class.SemiColon));
            return assignment_statement;
        }

        //done
        Node Declaration_statement()
        {
            // completed
            // Declaration_statement → Datatype Identifier_list ;


            if (Flag.out_of_count == true) { return null; }


            Node declaration_statement = new Node("Declaration_statement");
            declaration_statement.Children.Add(Datatype());
            declaration_statement.Children.Add(Declaration_statement_dash());
            return declaration_statement;
        }

        //done
        Node Declaration_statement_dash()
        {
            // completed
            // Declaration_statement → Datatype Identifier_list ;


            if (Flag.out_of_count == true) { return null; }


            Node declaration_statement_dash = new Node("Declaration_statement_dash");
            declaration_statement_dash.Children.Add(Identifier_list());
            declaration_statement_dash.Children.Add(Declaration_statement_dash_dash());
            return declaration_statement_dash;
        }

        //done
        Node Declaration_statement_dash_dash()
        {
            // completed
            // Declaration_statement → Datatype Identifier_list ;

            if (Flag.out_of_count == true) { return null; }

            Node declaration_statement_dash_dash = new Node("Declaration_statement_dash_dash");
            if(TokenStream[InputPointer].token_type == Token_Class.SemiColon)
            {
                declaration_statement_dash_dash.Children.Add(match(Token_Class.SemiColon));
            }
            else if(TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                declaration_statement_dash_dash.Children.Add(match(Token_Class.Comma));
                declaration_statement_dash_dash.Children.Add(Assignment_List());
            }
            return declaration_statement_dash_dash;
        }
        //done
        Node Assignment_List()
        {
            // completed
            // Declaration_statement → Datatype Identifier_list ;

            if (Flag.out_of_count == true) { return null; }

            Node assignment_List = new Node("Assignment_List");

            assignment_List.Children.Add(Assignment_statement());
            assignment_List.Children.Add(Assignment_List_dash());

            return assignment_List;
        }
        //done
        Node Assignment_List_dash()
        {
            // completed
            // Declaration_statement → Datatype Identifier_list ;

            if (Flag.out_of_count == true) { return null; }

            Node assignment_List_dash = new Node("Assignment_List_dash");
            if (TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                assignment_List_dash.Children.Add(match(Token_Class.Comma));
                assignment_List_dash.Children.Add(Assignment_statement());
                assignment_List_dash.Children.Add(Assignment_List_dash());
            }
            else
            {
                return null;
            }
            return assignment_List_dash;
        }

        //done
        Node Else_statement()
        {
            // completed
            // Else_statement → else Statements end | 𝜀


            if (Flag.out_of_count == true) { return null; }


            Node else_statement = new Node("Else_statement");
            else_statement.Children.Add(match(Token_Class.Else));
            else_statement.Children.Add(Statement_List());
            else_statement.Children.Add(match(Token_Class.End));

            return else_statement;
           
        }

        //done
        Node Else_if_statement()
        {
            // completed
            // Else_if_statement → elseif Condition_statement then Statements Ret_statement Else_statement end | �


            if (Flag.out_of_count == true) { return null; }


            Node else_if_statement = new Node("Else_if_statement");
            else_if_statement.Children.Add(match(Token_Class.Elseif));
            else_if_statement.Children.Add(Condition_statement());
            else_if_statement.Children.Add(match(Token_Class.Then));
            else_if_statement.Children.Add(Statement_List());
            if (TokenStream[InputPointer].token_type == Token_Class.Elseif)
            {
                else_if_statement.Children.Add(Else_if_statement());
            }
            if (TokenStream[InputPointer].token_type == Token_Class.Else)
            {
                else_if_statement.Children.Add(Else_statement());
            }
            if (TokenStream[InputPointer].token_type == Token_Class.End)
            {
                else_if_statement.Children.Add(match(Token_Class.End));
            }
         
            return else_if_statement;
            
          
        }

        //done
        Node If_statement()
        {
            // completed
            // If_statement → if Condition_statement then Statements Ret_statement Else_if_statement Else_statement end


            if (Flag.out_of_count == true) { return null; }


            Node if_statement = new Node("If_statement");
            if_statement.Children.Add(match(Token_Class.If));
            if_statement.Children.Add(Condition_statement());
            if_statement.Children.Add(match(Token_Class.Then));
            if_statement.Children.Add(Statement_List());
            if (TokenStream[InputPointer].token_type == Token_Class.Elseif)
            {
                if_statement.Children.Add(Else_if_statement());
            }
            if (TokenStream[InputPointer].token_type==Token_Class.Else)
            {
                if_statement.Children.Add(Else_statement());
            }
            if (TokenStream[InputPointer].token_type == Token_Class.End)
            {
                if_statement.Children.Add(match(Token_Class.End));
            }
            return if_statement;
        }

        //done
        Node Repeat_statement()
        {
            // completed
            // Repeat_statement → repeat Statements until Condition_statement


            if (Flag.out_of_count == true) { return null; }


            Node repeat_statement = new Node("Repeat_statement");
            repeat_statement.Children.Add(match(Token_Class.Repeat));
            repeat_statement.Children.Add(Statement_List());
            repeat_statement.Children.Add(match(Token_Class.Until));
            repeat_statement.Children.Add(Condition_statement());
            return repeat_statement;
        }

        // ============================================== Nour end =======================================================

        // ============================================== Ruq start =======================================================
        //done
        private Node Parameter()
        {
            if (Flag.out_of_count == true) { return null; }

            Node parameter = new Node("Parameter");

            // Parameter -> Datatype Identifier
            parameter.Children.Add(Datatype());
            parameter.Children.Add(match(Token_Class.Idenifier));

            return parameter;
        }

        //done
        Node Parameter_List()
        {
            if (Flag.out_of_count == true) { return null; }
            Node Parameter_List = new Node("Parameter_List");
            Parameter_List.Children.Add(Parameter());
            Parameter_List.Children.Add(Parameter_List_dash());
            return Parameter_List;
        }
        // ============================================== Ruq end =========================================================
        // ================================================================================================================
        // match : deal with tokens 

        //done
        private Node Parameter_List_dash()
        {
            Node parameter_List_dash = new Node("Parameter_List_dash");

            if (TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                parameter_List_dash.Children.Add(match(Token_Class.Comma));
                parameter_List_dash.Children.Add(Parameter());
                parameter_List_dash.Children.Add(Parameter_List_dash());
            }
            else
            {
                return null;
            }

            return parameter_List_dash;
        }
        public Node match(Token_Class ExpectedToken)
        {

            // if (Flag.isOfr == true && Flag.isSpecial == false) { return null; }

            if (InputPointer < TokenStream.Count)
            {
                if (ExpectedToken == TokenStream[InputPointer].token_type)
                {
                    InputPointer++;

                    if (InputPointer >= TokenStream.Count)
                    {
                        Flag.out_of_count = true;
                    }


                    Node newNode = new Node(ExpectedToken.ToString());

                    return newNode;
                }

                else
                {
                    Errors.Error_List_parser.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + " and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found\r\n");
                    InputPointer++;

                    if (InputPointer >= TokenStream.Count)
                    {
                        Flag.out_of_count = true;

                    }

                    return null;
                }
            }
            else
            {
                Errors.Error_List_parser.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + "\r\n");
                InputPointer++;

                return null;
            }
        }

        // ================================================================================================================
        // Print Parse Tree
        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}