// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta 
//               (Clarified meaning of dependent and dependee.)
//               (Clarified names in solution/project structure.)


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <author>
/// Method implementation written by Ryan Sloan
/// </author>
namespace SpreadsheetUtilities
{

    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {

        private Dictionary<string, HashSet<string>> _Dependents;

        private Dictionary<string, HashSet<string>> _Dependees;

        // private int to keep track of the number of ordered pairs in the DependencyGraph
        private int _OrderedPairs;

        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            _Dependents = new Dictionary<string, HashSet<string>>();
            _Dependees = new Dictionary<string, HashSet<string>>();
            _OrderedPairs = 0;
        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return _OrderedPairs; }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get
            {
                if (_Dependees.ContainsKey(s))
                    return _Dependees[s].Count;

                else
                    return 0;
            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            if (_Dependents.ContainsKey(s))
            {
                return _Dependents[s].Count > 0;
            }
            return false;
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (_Dependees.ContainsKey(s))
            {
                return _Dependees[s].Count > 0;
            }
            return false;
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (_Dependents.ContainsKey(s))
            {
                foreach (string dep in _Dependents[s])
                    yield return dep;
            }
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (_Dependees.ContainsKey(s))
            {
                foreach (string de in _Dependees[s])
                    yield return de;
            }
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {

            // Does the dependent side of the addDependency method
            // If Dependents already contain s then t is added to the hashSet represented by s
            if (_Dependents.ContainsKey(s))
            {
                if (_Dependents[s].Contains(t))
                {
                    // If the ordered pair already exist in the DependencyGraph then return
                    return;
                }
                else
                {
                    _Dependents[s].Add(t);
                    _OrderedPairs++;
                }
            }
            // Creates a new key value pair
            else
            {
                _Dependents.Add(s, new HashSet<string>());
                _Dependents[s].Add(t);
                _OrderedPairs++;
            }



            // Does the dependee side of the addDependency method
            if (_Dependees.ContainsKey(t))
            {
                _Dependees[t].Add(s);
            }
            else
            {
                _Dependees.Add(t, new HashSet<string>());
                _Dependees[t].Add(s);
            }
        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            // Checks if s is in the dependency graph
            if (_Dependents.ContainsKey(s))
            {
                // Cheecks if s and t are pairs then removes the pair if true
                if (_Dependents[s].Contains(t))
                {
                    _Dependents[s].Remove(t);
                    _Dependees[t].Remove(s);
                    _OrderedPairs--;
                }
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (_Dependents.ContainsKey(s))
            {
                // Create a new HashSet to avoid editing something that is being iterated through
                HashSet<string> thisDependents = new HashSet<string>(_Dependents[s]);

                // Iterates through all the dependents of s and removes the ordered pair from the graph
                foreach (string oldDep in thisDependents)
                    RemoveDependency(s, oldDep);
            }

            // Iterates through the new dependents and adds all the new ordered pairs
            foreach (string newDep in newDependents)
                AddDependency(s, newDep);
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {

            if (_Dependees.ContainsKey(s))
            {
                // Create a new HashSet to avoid editing something that is being iterated through
                HashSet<string> thisDependees = new HashSet<string>(_Dependees[s]);

                // Iterates through all the dependees of s and removes the ordered pair from the graph
                foreach (string oldDee in thisDependees)
                    RemoveDependency(oldDee, s);
            }

            // Iterates through the new dependees and adds all the new ordered pairs
            foreach(string newDee in newDependees)
                AddDependency(newDee, s);
        }

    }

}
