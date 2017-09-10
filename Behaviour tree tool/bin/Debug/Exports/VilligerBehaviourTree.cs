using System;
using System.Collections.Generic;

/*
* class: BehaviourTree
* --------------------
*
* ____________________
*
* Author: ____________
*/
public class BehaviourTree
{

      /*
      * class: TreeNode
      * ---------------
      *
      * _______________
      *
      * Author: _______
      */
      public class TreeNode
      {
          //the list of the nodes children
          public List<TreeNode> m_children = new List<TreeNode>();
          
          //the delegate function we will assign to exacute
          public delegate bool Run();
          public Run run;
      }

      //List of all the nodes making up this tree
      private List<TreeNode> m_nodes = new List<TreeNode>();
      
      //the node at the begining of the tree
      private TreeNode m_startNode = null;

      /*
      * Function: Constructor
      * ---------------------
      *
      * Default constructor
      *
      */
      public BehaviourTree()
      {
          SetUpNodes();
      }

      /*
      * Function: SetUpNodes
      * ---------------------
      *
      * this function sets up all the nodes in the tree
      * assiging them there functions and linking them up
      *
      * Parameters: none
      *
      *returns: returns nothing as it is a void function
      */
      public void SetUpNodes()
      {
          TreeNode n0 = new TreeNode();
          m_nodes.Add(n0);
          
          m_startNode = n0;
          
          TreeNode n1 = new TreeNode();
          m_nodes.Add(n1);
          
          TreeNode n2 = new TreeNode();
          m_nodes.Add(n2);
          
          TreeNode n3 = new TreeNode();
          m_nodes.Add(n3);
          
          TreeNode n4 = new TreeNode();
          m_nodes.Add(n4);
          
          TreeNode n5 = new TreeNode();
          m_nodes.Add(n5);
          
          TreeNode n9 = new TreeNode();
          m_nodes.Add(n9);
          
          TreeNode n15 = new TreeNode();
          m_nodes.Add(n15);
          
          TreeNode n16 = new TreeNode();
          m_nodes.Add(n16);
          
          //////////////////////////////////////////////////////
          //////////////////////////////////////////////////////
      
          //Do Other thing!
          n0.run = () =>
          {
          
             //code for desiered node action here
             
             return true;
          };
          
          //Do thing!
          n1.run = () =>
          {
          
             //code for desiered node action here
             
             return true;
          };
          
          //thing?
          n2.run = () =>
          {
             
             //code for desiered node check here
             if(true)
             {
                 return true;
             }
             
             return false;
          };
          
          //Testlingz
          n3.run = () =>
          {
             
             //runs through its children Nodes and returns true for the first child that returns true, used to reform or logic gates
             foreach(TreeNode n in n3.m_children)
             {
                 if(n.run() == true)
                 {
                     return true;
                 }
             }
             
             return true;
          };
          
          //Start Node
          n4.run = () =>
          {
             
             //runs through all its children Nodes and returns true if they all return true, used to preform and logic gates
             foreach(TreeNode n in n4.m_children)
             {
                 if(n.run() == false)
                 {
                     return false;
                 }
             }
             
             return false;
          };
          
          //bad logic
          n5.run = () =>
          {
             Random r = new Random();
             int i = r.Next(0, n5.m_children.Count() + 1);
             int count = 0;
             
             //runs through its children Nodes assuming the delegator value check is true
             foreach(TreeNode n in n5.m_children)
             {
                 if(count == i)
                 {
                     return n.run();
                 }
                 count++;
             }
             
              return true;
          };
          
          //A Bad Node
          n9.run = () =>
          {
          
             //code for desiered node action here
             
             return true;
          };
          
          //Default Description
          n15.run = () =>
          {
             
             //runs through its children Nodes assuming the delegator value check is true
             if(true)
             {
                 foreach(TreeNode n in n15.m_children)
                 {
                     n.run();
                 }
             }
             
             //Implament your own true false logic here
             return true;
          };
          
          //Default Description
          n16.run = () =>
          {
          
             //code for desiered node action here
             
             return true;
          };
          
          //////////////////////////////////////////////////////
          //////////////////////////////////////////////////////
      
          n3.m_children.Add(n0);
          n3.m_children.Add(n1);
          n3.m_children.Add(n2);
          
          n4.m_children.Add(n3);
          n4.m_children.Add(n5);
          n4.m_children.Add(n15);
          
          n5.m_children.Add(n9);
          
          n15.m_children.Add(n16);
          
      }

      /*
      * Function: Update
      * ----------------
      *
      * this function runs the behaviour tree each time it is called
      *
      * Parameters: none
      *
      *returns: returns nothing as it is a void function
      */
      public void Update()
      {
          
          m_startNode.run();
          
      }
}
