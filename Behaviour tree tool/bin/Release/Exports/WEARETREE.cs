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
          
          //////////////////////////////////////////////////////
          //////////////////////////////////////////////////////
      
          n3.m_children.Add(n0);
          n3.m_children.Add(n1);
          n3.m_children.Add(n2);
          
          //////////////////////////////////////////////////////
          //////////////////////////////////////////////////////
      
          //Default Description
          n0.run = () =>
          {
          
             //code for desiered node action here
             
             return true;
          };
          
          //Default Description
          n1.run = () =>
          {
          
             //code for desiered node action here
             
             return true;
          };
          
          //Default Description
          n2.run = () =>
          {
          
             //code for desiered node action here
             
             return true;
          };
          
          //Default Description
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
