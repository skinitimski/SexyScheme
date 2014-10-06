using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Atmosphere.Extensions;
using Atmosphere.SexyLib.Exceptions;

namespace Atmosphere.SexyLib
{
    public class List
    {
        public static readonly List Empty = new List();


        public List()
        {
            Members = new List<ISExp>();
        }






        public override bool Equals(object obj)
        {
            bool equals = false;

            if (obj != null)
            {
                List that = obj as List;

                if (that != null)
                {
                    if (this.Count == that.Count)
                    {
                        bool membersEqual = true;

                        for (int i = 0; i < this.Count; i++)
                        {
                            if (!this.Members[i].Equals(that.Members[i]))
                            {
                                membersEqual = false;
                                break;
                            }
                        }

                        equals = membersEqual;
                    }
                }
            }

            return equals;
        }

        public ISExp GetAt(int index)
        {
            if (index < 0 || index >= Count) throw new ArgumentOutOfRangeException("index");

            return Members[index];
        }

        public T GetAt<T>(int index)
        {
            T ret = default(T);

            ISExp member = GetAt(index);

            try
            {
                if (member.IsAtom)
                {
                    ret = (T)((Atom)member).Value;
                }
                else // is list
                {
                    ret = (T)member;
                }
            }
            catch (InvalidCastException)
            {
                Type actualType = typeof(List);

                if (member.IsAtom)
                {
                    actualType = ((Atom)member).Value.GetType();
                }

                throw new WrongTypeException(typeof(T), actualType);
            }

            return ret;
        }




        private List<ISExp> Members { get; set; }



        public ISExp Car
        {
            get
            {
                if (IsEmpty) throw new UndefinedOperationException("Cannot take the car of an empty list.");

                return Members[0];
            }
        }

        public List Cdr
        {
            get
            {
                if (IsEmpty) throw new UndefinedOperationException("Cannot take the cdr of an empty list.");
                     
                List cdr = new List();

                foreach (ISExp member in Members.Skip(1))
                {
                    cdr.AppendMember(member);
                }

                return cdr;
            }
        }

        public bool IsAtom { get { return false; } }
    }
}

