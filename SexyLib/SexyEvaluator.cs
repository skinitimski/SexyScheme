using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Atmosphere.Extensions;
using Atmosphere.SexyLib.Exceptions;

namespace Atmosphere.SexyLib
{
    public class SexyEvaluator
    {






        public SexyEvaluator()
        {
            Closure = Closure.Create();

            AddPrimitives();
        }




        #region Add Primitives

        private void AddPrimitives()
        {            
            Closure.AddSymbolDefinition("true", Atom.True);
            Closure.AddSymbolDefinition("false", Atom.False);
            
            Closure.AddSymbolDefinition("if", Atom.CreatePrimitive(Primitives.If));
            Closure.AddSymbolDefinition("and", Atom.CreatePrimitive(Primitives.And));
            Closure.AddSymbolDefinition("or", Atom.CreatePrimitive(Primitives.Or));
            Closure.AddSymbolDefinition("not", Atom.CreatePrimitive(Primitives.Not));
            
            //Closure.AddSymbolDefinition("list?", Atom.CreatePrimitive(Primitives.ListP));
            Closure.AddSymbolDefinition("pair?", Atom.CreatePrimitive(Primitives.PairP));
            Closure.AddSymbolDefinition("boolean?", Atom.CreatePrimitive(Primitives.BooleanP));
            Closure.AddSymbolDefinition("char?", Atom.CreatePrimitive(Primitives.CharP));
            Closure.AddSymbolDefinition("string?", Atom.CreatePrimitive(Primitives.StringP));
            Closure.AddSymbolDefinition("symbol?", Atom.CreatePrimitive(Primitives.SymbolP));
            Closure.AddSymbolDefinition("number?", Atom.CreatePrimitive(Primitives.NumberP));
            Closure.AddSymbolDefinition("integer?", Atom.CreatePrimitive(Primitives.IntegerP));
            
            Closure.AddSymbolDefinition("+", Atom.CreatePrimitive(Primitives.Add));            
            Closure.AddSymbolDefinition("-", Atom.CreatePrimitive(Primitives.Subtract));       
            Closure.AddSymbolDefinition("*", Atom.CreatePrimitive(Primitives.Multiply));       
            Closure.AddSymbolDefinition("/", Atom.CreatePrimitive(Primitives.Divide));
            Closure.AddSymbolDefinition("modulo", Atom.CreatePrimitive(Primitives.Modulo));
            Closure.AddSymbolDefinition("expt", Atom.CreatePrimitive(Primitives.Exponent));
            
            Closure.AddSymbolDefinition("car", Atom.CreatePrimitive(Primitives.Car));
            Closure.AddSymbolDefinition("cdr", Atom.CreatePrimitive(Primitives.Cdr));
            Closure.AddSymbolDefinition("cons", Atom.CreatePrimitive(Primitives.Cons));
            //Closure.AddSymbolDefinition("list", Atom.CreatePrimitive(Primitives.List));

            Closure.AddSymbolDefinition("write", Atom.CreatePrimitive(Primitives.Write));
            Closure.AddSymbolDefinition("display", Atom.CreatePrimitive(Primitives.Display));
            Closure.AddSymbolDefinition("exit", Atom.CreatePrimitive(Primitives.Exit));
        }

        #endregion Add Primitives



        public ISExp Eval(ISExp sexp)
        {
            return Eval(sexp, Closure, 0);
        }



        private static ISExp Eval(ISExp sexp, Closure closure, int depth)
        {
            ISExp result;

            if (sexp.IsAtom)
            {
                result = EvalAtom((Atom)sexp, closure, depth);
            }
            else
            {
                result = EvalList((Pair)sexp, closure, depth);
            }

            return result;
        }

        private static ISExp EvalAtom(Atom atom, Closure closure, int depth)
        {
            ISExp evaluated = default(ISExp);
            
            switch (atom.Type)
            {
                case AtomType.BOOLEAN:
                case AtomType.CHAR:
                case AtomType.DOUBLE:
                case AtomType.LONG:
                case AtomType.LAMBDA:
                case AtomType.PRIMITIVE:
                case AtomType.STRING:
                case AtomType.OBJECT:

                    evaluated = atom;
                    break;

                case AtomType.SYMBOL:
                    
                    if (atom.Value.Equals("."))
                    {
                        throw new DotException();
                    }

                    string symbol = (string)atom.Value;

                    symbol = symbol.ToLower();

                    evaluated = closure.Resolve(symbol);

                    break;

                default:
                    throw new Exception(String.Format("No support yet for type {0}", atom.Type));
            }
            
            return evaluated;
        }

        private static ISExp EvalList(Pair list, Closure closure, int depth)
        {
            if (list.IsEmpty)
            {
                return list;
            }
                            
            if (list.Car.IsAtom)
            {
                Atom car = list.Car as Atom;
                    
                if (car.Equals(Atom.KeywordLambda))
                {
                    return EvalLambda(list);
                }
                else if (car.Equals(Atom.KeywordQuote))
                {
                    return EvalQuote(list);
                }
                else if (car.Equals(Atom.KeywordDefine))
                {
                    return EvalDefine(list, closure, depth);
                }
                else if (car.Equals(Atom.KeywordDefineSyntax))
                {
                    return EvalDefineSyntax(list, closure, depth);
                }
                else if (car.Equals(Atom.KeywordSet))
                {
                    return EvalSet(list, closure, depth);
                }
            }               

            return EvalApplication(list, closure, depth);
        }



        private static ISExp EvalApplication(Pair pair, Closure closure, int depth)
        { 
            depth++;
            
            Atom proc = Eval(pair.Car, closure, depth) as Atom;
            
            if (proc == null && !(proc.Type == AtomType.PRIMITIVE || proc.Type == AtomType.LAMBDA))
            {
                throw new ProcedureApplicationException(proc);
            }
            
            List<ISExp> argsList = new List<ISExp>();
            
            while (!((Pair)pair.Cdr).IsEmpty)
            {
                pair = pair.Cdr as Pair;
                
                argsList.Add(pair.Car);
            } 
            
            ISExp[] args = argsList.ToArray();
            
            ISExp result = default(ISExp);
            
            if (proc.Type == AtomType.PRIMITIVE)
            {
                // Need to evaluate each argument before passing.
                for (int i = 0; i < args.Length; i++)
                {
                    args[i] = Eval(args[i], closure, depth);
                }
                
                result = ((Primitive)proc.Value).Invoke(args);
            }
            else // proc.Type == AtomType.LAMBDA
            {
                Lambda lambda = (Lambda)proc.Value;
                Closure local = lambda.GenerateClosure(closure, args);
                
                foreach (ISExp sexp in lambda.Body)
                {
                    // return last one evaluated
                    result = Eval(sexp, local, depth);
                }
            }
            
            return result;
        }



        private static ISExp EvalLambda(Pair pair)
        {            
            List<Atom> identifiers = new List<Atom>();
            List<ISExp> body = new List<ISExp>();
            bool variableArity = false;
            
            // (lambda <formals> <body>)
            // <formals> :
            //     (variable ...)
            //     variable
            //     (variable1 ... variableN . variableN+1)
            // <body> :
            //     expr ...
            
            
            Pair lambdaParts = pair.Cdr as Pair;
            
            if (lambdaParts == null || lambdaParts.Car == null)
            {
                throw new BadSyntaxException("lambda", pair);
            }
            
            ISExp formals = lambdaParts.Car;
            
            if (formals.IsAtom)
            {
                variableArity = true;
                
                Atom identifier = formals as Atom;
                
                if (identifier == null || identifier.Type != AtomType.SYMBOL)
                {
                    throw new LambdaException("Not an identifier: {0}", identifier);
                }
                
                identifiers.Add((Atom)formals);
            }
            else
            {
                Pair formalsPair = formals as Pair;
                
                while (!formalsPair.IsEmpty)
                {
                    Atom identifier = formalsPair.Car as Atom;
                    
                    if (identifier != null && ((Atom)identifier).Type == AtomType.SYMBOL)
                    {
                        identifiers.Add(identifier);
                    }
                    else
                    {
                        throw new LambdaException("Not an identifier: {0}", formalsPair.Car);
                    }
                    
                    formalsPair = formalsPair.Cdr as Pair;
                    
                    if (formalsPair == null)
                    {
                        throw new LambdaException("Formals expected to be proper list, given {0}", formals);
                    }
                }
            }
            
            // assemble body
            
            Pair lambdaBody = lambdaParts.Cdr as Pair;
            
            while (!lambdaBody.IsEmpty)
            {
                body.Add(lambdaBody.Car);
                
                lambdaBody = lambdaBody.Cdr as Pair;
                
                if (lambdaBody == null)
                {
                    throw new LambdaException("Body expected to be proper list, given {0}", lambdaParts.Cdr);
                }
            }

            if (body.Count == 0)
            {
                throw new BadSyntaxException("lambda", pair);
            }
            
            if (variableArity)
            {
                return Atom.CreateLambda(identifiers[0], body);
            }
            else
            {
                return Atom.CreateLambda(identifiers, body);
            }
        }


        private static ISExp EvalQuote(Pair pair)
        {
            Pair quoteParts = pair.Cdr as Pair;
            
            if (quoteParts == null || quoteParts.Car == null || !quoteParts.Cdr.Equals(Pair.Empty))
            {
                throw new BadSyntaxException("quote", pair);
            }
            
            return quoteParts.Car;
        }

        private static ISExp EvalDefine(Pair list, Closure closure, int depth)
        {
            if (depth != 0)
            {
                throw new DefineException("Not allowed in expression context; must be at top-level.");
            }
            
            Pair defineParts = list.Cdr as Pair;
            
            if (defineParts == null || defineParts.Car == null)
            {
                throw new BadSyntaxException("define", list);
            }
            
            Atom identifier = defineParts.Car as Atom;
            
            if (identifier == null || !identifier.Type.Equals(AtomType.SYMBOL))
            {
                throw new BadSyntaxException("define", list, "cadr must be an identifier");
            }
            
            Pair defineValue = defineParts.Cdr as Pair;
            
            if (defineValue == null || !defineValue.Cdr.Equals(Pair.Empty))
            {
                throw new BadSyntaxException("define", list);
            }
            
            
            ISExp value = defineValue.Car;
            
            value = Eval(value, closure, depth + 1);
            
            closure.Define((string)identifier.Value, value);

            return Atom.Null;
        }

        private static ISExp EvalDefineSyntax(Pair list, Closure closure, int depth)
        {
            if (depth != 0)
            {
                throw new DefineSyntaxException("Not allowed in expression context; must be at top-level.");
            }

            throw new NotImplementedException("define-syntax not yet implemented.");
        }

        private static ISExp EvalSet(Pair list, Closure closure, int depth)
        {
            Pair setParts = list.Cdr as Pair;

            if (setParts == null || setParts.Car == null)
            {
                throw new BadSyntaxException("set!", list);
            }

            Atom identifier = setParts.Car as Atom;
            
            if (identifier == null || !identifier.Type.Equals(AtomType.SYMBOL))
            {
                throw new BadSyntaxException("set!", list, "cadr must be an identifier");
            }

            Pair setValue = setParts.Cdr as Pair;

            if (setValue == null || !setValue.Cdr.Equals(Pair.Empty))
            {
                throw new BadSyntaxException("set!", list);
            }


            ISExp value = setValue.Car;

            value = Eval(value, closure, depth + 1);

            closure.Bind((string)identifier.Value, value);

            return Atom.Null;
        }






//        public static ISExp IsMatch(ISExp pattern, ISExp expression)
//        {
//        }



        public Closure Closure { get; private set; }
    }
}

