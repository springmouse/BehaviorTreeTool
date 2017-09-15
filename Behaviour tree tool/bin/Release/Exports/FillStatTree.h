#pragma once
#include <list>
#include <functional>

/*
* class: BehaviourTree
* --------------------
*
* ____________________
*
* Author: ____________
*/
class BehaviourTree
{
public:
      /*
      * class: TreeNode
      * ---------------
      *
      * _______________
      *
      * Author: _______
      */
      class TreeNode
      {
      public:
          //the list of the nodes children
          std::list<TreeNode *> m_children;
          
          //the delegate function we will assign to exacute
          std::function<bool()> run;
      };

      //List of all the nodes making up this tree
      std::list<TreeNode *> m_nodes;
      
      //the node at the begining of the tree
      TreeNode * m_startNode = nullptr;

      /*
      * Function: Constructor
      * ---------------------
      *
      * Default constructor
      *
      */
      BehaviourTree()
      {
          SetUpNodes();
      };

      /*
      * Function: DeConstructor
      * ---------------------
      *
      * Default DeConstructor
      *
      */
      ~BehaviourTree()
      {
          m_nodes.clear();
      };

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
      void SetUpNodes()
      {
          TreeNode * n0 = new TreeNode();
          m_nodes.push_back(n0);
          
          m_startNode = n0;
          
          TreeNode * n1 = new TreeNode();
          m_nodes.push_back(n1);
          
          TreeNode * n2 = new TreeNode();
          m_nodes.push_back(n2);
          
          TreeNode * n4 = new TreeNode();
          m_nodes.push_back(n4);
          
          TreeNode * n6 = new TreeNode();
          m_nodes.push_back(n6);
          
          TreeNode * n7 = new TreeNode();
          m_nodes.push_back(n7);
          
          TreeNode * n8 = new TreeNode();
          m_nodes.push_back(n8);
          
          TreeNode * n9 = new TreeNode();
          m_nodes.push_back(n9);
          
          TreeNode * n12 = new TreeNode();
          m_nodes.push_back(n12);
          
          TreeNode * n13 = new TreeNode();
          m_nodes.push_back(n13);
          
          //////////////////////////////////////////////////////
          //////////////////////////////////////////////////////
      
          //fill stats
          n0->run = [n0]()->bool
          {
             
             //runs through its children Nodes and returns true for the first child that returns true, used to reform or logic gates
             for each(TreeNode * n in n0->m_children)
             {
                 if(n->run() == true)
                 {
                     return true;
                 }
             }
             
             return true;
          };
          
          //Check hunger
          n1->run = [n1]()->bool
          {
             
             //runs through all its children Nodes and returns true if they all return true, used to preform and logic gates
             for each(TreeNode * n in n1->m_children)
             {
                 if(n->run() == false)
                 {
                     return false;
                 }
             }
             
             return false;
          };
          
          //Check thirst
          n2->run = [n2]()->bool
          {
             
             //runs through all its children Nodes and returns true if they all return true, used to preform and logic gates
             for each(TreeNode * n in n2->m_children)
             {
                 if(n->run() == false)
                 {
                     return false;
                 }
             }
             
             return false;
          };
          
          //Hunger < 50
          n4->run = [n4]()->bool
          {
             
             //code for desiered node check here
             if(true)
             {
                 return true;
             }
             
             return false;
          };
          
          //Drink from bottle
          n6->run = [n6]()->bool
          {
          
             //code for desiered node action here
             
             return true;
          };
          
          //thirst < 50
          n7->run = [n7]()->bool
          {
             
             //code for desiered node check here
             if(true)
             {
                 return true;
             }
             
             return false;
          };
          
          //is food with in range
          n8->run = [n8]()->bool
          {
             
             //runs through all its children Nodes and returns true if they all return true, used to preform and logic gates
             for each(TreeNode * n in n8->m_children)
             {
                 if(n->run() == false)
                 {
                     return false;
                 }
             }
             
             return false;
          };
          
          //Gather food
          n9->run = [n9]()->bool
          {
          
             //code for desiered node action here
             
             return true;
          };
          
          //move towards food
          n12->run = [n12]()->bool
          {
          
             //code for desiered node action here
             
             return true;
          };
          
          //food.pos < range
          n13->run = [n13]()->bool
          {
             
             //code for desiered node check here
             if(true)
             {
                 return true;
             }
             
             return false;
          };
          
          //////////////////////////////////////////////////////
          //////////////////////////////////////////////////////
      
          n0->m_children.push_back(n1);
          n0->m_children.push_back(n2);
          
          n1->m_children.push_back(n4);
          n1->m_children.push_back(n8);
          
          n2->m_children.push_back(n7);
          n2->m_children.push_back(n6);
          
          n8->m_children.push_back(n9);
          n8->m_children.push_back(n12);
          n8->m_children.push_back(n13);
          
      };

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
      void Update()
      {
          
          m_startNode->run();
          
      };
};
