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
          TreeNode n12 = new TreeNode();
          m_nodes.Add(n12);
          
          m_startNode = n12;
          
          TreeNode n13 = new TreeNode();
          m_nodes.Add(n13);
          
          TreeNode n14 = new TreeNode();
          m_nodes.Add(n14);
          
          TreeNode n15 = new TreeNode();
          m_nodes.Add(n15);
          
          TreeNode n16 = new TreeNode();
          m_nodes.Add(n16);
          
          TreeNode n17 = new TreeNode();
          m_nodes.Add(n17);
          
          TreeNode n18 = new TreeNode();
          m_nodes.Add(n18);
          
          //////////////////////////////////////////////////////
          //////////////////////////////////////////////////////
      
          n12.m_children.Add(n13);
          n12.m_children.Add(n14);
          
          n13.m_children.Add(n17);
          n13.m_children.Add(n15);
          
          n14.m_children.Add(n18);
          n14.m_children.Add(n16);
          
          //////////////////////////////////////////////////////
          //////////////////////////////////////////////////////
      
          //chase
          n12.run = () =>
          {
             
             //runs through all its children Nodes and returns true if they all return true, used to preform and logic gates
             foreach(TreeNode n in n12.m_children)
             {
                 if(n.run() == false)
                 {
                     return false;
                 }
             }
             
             return false;
          };
          
          //check enemy close
          n13.run = () =>
          {
             
             //runs through all its children Nodes and returns true if they all return true, used to preform and logic gates
             foreach(TreeNode n in n13.m_children)
             {
                 if(n.run() == false)
                 {
                     return false;
                 }
             }
             
             return false;
          };
          
          //Default Description
          n14.run = () =>
          {
             
             //runs through all its children Nodes and returns true if they all return true, used to preform and logic gates
             foreach(TreeNode n in n14.m_children)
             {
                 if(n.run() == false)
                 {
                     return false;
                 }
             }
             
             return false;
          };
          
          //move to enemy
          n15.run = () =>
          {
          
             //code for desiered node action here
             
             return true;
          };
          
          //attack enemy
          n16.run = () =>
          {
          
             //code for desiered node action here
             
             return true;
          };
          
          //check if can see enemy
          n17.run = () =>
          {
             
             //code for desiered node check here
             if(true)
             {
                 return true;
             }
             
             return false;
          };
          
          //enemy within attack range
          n18.run = () =>
          {
             
             //code for desiered node check here
             if(true)
             {
                 return true;
             }
             
             return false;
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
