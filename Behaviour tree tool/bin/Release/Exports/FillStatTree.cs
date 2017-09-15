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
          
          TreeNode n4 = new TreeNode();
          m_nodes.Add(n4);
          
          TreeNode n6 = new TreeNode();
          m_nodes.Add(n6);
          
          TreeNode n7 = new TreeNode();
          m_nodes.Add(n7);
          
          TreeNode n8 = new TreeNode();
          m_nodes.Add(n8);
          
          TreeNode n9 = new TreeNode();
          m_nodes.Add(n9);
          
          TreeNode n12 = new TreeNode();
          m_nodes.Add(n12);
          
          TreeNode n13 = new TreeNode();
          m_nodes.Add(n13);
          
          //////////////////////////////////////////////////////
          //////////////////////////////////////////////////////
      
          n0.m_children.Add(n1);
          n0.m_children.Add(n2);
          
          n1.m_children.Add(n4);
          n1.m_children.Add(n8);
          
          n2.m_children.Add(n7);
          n2.m_children.Add(n6);
          
          n8.m_children.Add(n9);
          n8.m_children.Add(n12);
          n8.m_children.Add(n13);
          
          //////////////////////////////////////////////////////
          //////////////////////////////////////////////////////
      
          //fill stats
          n0.run = () =>
          {
             
             //runs through its children Nodes and returns true for the first child that returns true, used to reform or logic gates
             foreach(TreeNode n in n0.m_children)
             {
                 if(n.run() == true)
                 {
                     return true;
                 }
             }
             
             return true;
          };
          
          //Check hunger
          n1.run = () =>
          {
             
             //runs through all its children Nodes and returns true if they all return true, used to preform and logic gates
             foreach(TreeNode n in n1.m_children)
             {
                 if(n.run() == false)
                 {
                     return false;
                 }
             }
             
             return false;
          };
          
          //Check thirst
          n2.run = () =>
          {
             
             //runs through all its children Nodes and returns true if they all return true, used to preform and logic gates
             foreach(TreeNode n in n2.m_children)
             {
                 if(n.run() == false)
                 {
                     return false;
                 }
             }
             
             return false;
          };
          
          //Hunger < 50
          n4.run = () =>
          {
             
             //code for desiered node check here
             if(true)
             {
                 return true;
             }
             
             return false;
          };
          
          //Drink from bottle
          n6.run = () =>
          {
          
             //code for desiered node action here
             
             return true;
          };
          
          //thirst < 50
          n7.run = () =>
          {
             
             //code for desiered node check here
             if(true)
             {
                 return true;
             }
             
             return false;
          };
          
          //is food with in range
          n8.run = () =>
          {
             
             //runs through all its children Nodes and returns true if they all return true, used to preform and logic gates
             foreach(TreeNode n in n8.m_children)
             {
                 if(n.run() == false)
                 {
                     return false;
                 }
             }
             
             return false;
          };
          
          //Gather food
          n9.run = () =>
          {
          
             //code for desiered node action here
             
             return true;
          };
          
          //move towards food
          n12.run = () =>
          {
          
             //code for desiered node action here
             
             return true;
          };
          
          //food.pos < range
          n13.run = () =>
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
