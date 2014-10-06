using System;
using System.Collections.Generic;
using System.Linq;

using Atmosphere.SexyLib.Exceptions;

namespace Atmosphere.SexyLib
{
    /// <summary>
    /// Represents a generic Lambda expression.
    /// </summary>
    public abstract class Lambda
    {
        private ISExp[] _body;

        private Lambda()
        {
        }

        protected Lambda(List<ISExp> body) : this()
        {
            _body = body.ToArray();
        }

//        public ISExp Invoke(Closure closure, params ISExp[] parameters)
//        {
//            Closure local = GenerateClosure(closure, parameters);
//
//            return EvaluateBody(local);
//        }

        public abstract Closure GenerateClosure(Closure parent, params ISExp[] parameters);

//        private ISExp EvaluateBody(Closure closure)
//        {
//            return _body[_body.Length - 1].Eval(closure);
//        }


        protected String BodyRepresentation { get { return Body.Select(x => x.ToString()).Aggregate((x, y) => x + " " + y); } }

        public List<ISExp> Body { get { return _body.ToList(); } }
    }

    /// <summary>
    /// Represents a lambda expression of the form (lambda (x y ...) <body>).
    /// </summary> 
    public class FixedLambda : Lambda
    {
        private string[] _formals;

        /// <summary>
        /// Initializes a new instance of the <see cref="Atmosphere.SexyLib.FixedLambda"/> class.
        /// </summary>
        /// <param name='formals'>List of atoms (symbols) representing the fixed-width parameters (variable names)</param>
        /// <param name='body'>Body of the lambda expression</param>
        public FixedLambda(List<Atom> formals, List<ISExp> body)
            : base(body)
        {
            _formals = formals.Select(x => (string)x.Value).ToArray();
        }

        public override Closure GenerateClosure(Closure closure, params ISExp[] parameters)
        {
            if (parameters.Length != _formals.Length)
            {
                throw new ArityException("#<procedure>", _formals.Length, parameters.Length);
            }

            Closure child = Closure.Create(closure);

            for (int i = 0; i < _formals.Length; i++)
            {
                child.AddSymbolDefinition(_formals[i], parameters[i]);
            }
            
            return child;
        }
        
        public override string ToString()
        {
            return String.Format("(lambda ({0}) {1})", _formals.Length == 0 ? String.Empty : _formals.Aggregate((x, y) => x + " " + y), BodyRepresentation);
        }
        
        public IList<string> Formals { get { return _formals.ToList(); } }
    }
    
    /// <summary>
    /// Represents a lambda expression of the form (lambda x <body>).
    /// </summary> 
    public class VariableLambda : Lambda
    {        
        /// <summary>
        /// Initializes a new instance of the <see cref="Atmosphere.SexyLib.VariableLambda"/> class.
        /// </summary>
        /// <param name='formals'>Atom (symbol) representing the variable to which parameters will be bound as a list</param>
        /// <param name='body'>Body of the lambda expression</param>
        public VariableLambda(Atom formal, List<ISExp> body)
            : base(body)
        {
            Formal = (string)formal.Value;
        }
        
        public override Closure GenerateClosure(Closure closure, params ISExp[] parameters)
        {
            Pair parametersAsList = Pair.List(parameters);
                        
            Closure child = Closure.Create(closure);

            child.AddSymbolDefinition(Formal, parametersAsList);
            
            return child;
        }
        
        public override string ToString()
        {
            return String.Format("(lambda {0} {1})", Formal, BodyRepresentation);
        }

        public string Formal { get; private set; }
    }
    
    /// <summary>
    /// Represents a lambda expression of the form (lambda (x y ... . z) <body>).
    /// </summary> 
    public class RestLambda : Lambda
    {
        private string[] _formals;

        /// <summary>
        /// Initializes a new instance of the <see cref="Atmosphere.SexyLib.RestLambda"/> class.
        /// </summary>
        /// <param name='formals'>List of atoms (symbols) representing the fixed-width parameters (variable names)</param>
        /// <param name='restFormal'>Atom (symbol) representing the "rest" variable name</param>
        /// <param name='body'>Body of the lambda expression</param>
        public RestLambda(List<Atom> formals, Atom restFormal, List<ISExp> body)
            : base(body)
        {
            _formals = formals.Select(x => (string)x.Value).ToArray();
            RestFormal = (string)restFormal.Value;
        }

        public override Closure GenerateClosure(Closure closure, params ISExp[] parameters)
        {
            if (parameters.Length < _formals.Length)
            {
                throw new NotEnoughArgumentsException("#<procedure>", _formals.Length, parameters.Length);
            }

            Closure child = Closure.Create(closure);

            for (int i = 0; i < _formals.Length; i++)
            {
                child.AddSymbolDefinition(Formals[i], parameters[i]);
            }

            ISExp[] restParameters = new ISExp[parameters.Length - _formals.Length];

            Array.Copy(parameters, _formals.Length, restParameters, 0, restParameters.Length);

            Pair rest = Pair.List(restParameters);

            child.AddSymbolDefinition(RestFormal, rest);
            
            return child;
        }

        public override string ToString()
        {
            return String.Format("(lambda ({0} . {1}) {2})",
                                       Formals.Aggregate((x, y) => x + " " + y),
                                       RestFormal,
                                       BodyRepresentation);
        }

        public IList<string> Formals { get { return _formals.ToList(); } }

        public string RestFormal { get; private set; }
    }
}

