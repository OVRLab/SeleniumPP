using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/////////////////////////////
using System.Windows.Automation;
using System.Windows;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Windows.Forms;

namespace computer_assisted_instruction
{
    public class AppTree
    {
        string AppAutomationId = string.Empty;
        int ProcessID = 0;
        UITree uiTree = new UITree();
        TreeNode appTree = null;
        public AppTree(string _AppAutomationId)
        {
            AppAutomationId = _AppAutomationId;
        }
        public AppTree(int _ProcessID)
        {
            ProcessID = _ProcessID;
        }
        
        public void BuildAppTree(Point pt)
        {
            AutomationElement currentElm = uiTree.GetElementFromPoint(pt);
            if (ProcessID != 0 && currentElm.Current.ProcessId != ProcessID)
                return;
            ProcessID = currentElm.Current.ProcessId;
            TreeNode tn = uiTree.WalkUpRawElements(currentElm, 100);
            if (!(AppAutomationId != string.Empty &&
                tn.Nodes[0].Text.Split(new string[1] { "##" }, StringSplitOptions.RemoveEmptyEntries)[0] == AppAutomationId))
                return;
            /////////////////////////////////
            //Build App Tree
            if (appTree == null)
                appTree = tn;
            else
                appTree = mergTreeNode(appTree, tn);
        }

        private TreeNode mergTreeNode(TreeNode tree1, TreeNode tree2)
        {
            if (tree1.Text == tree2.Text &&
                tree1.Nodes[0].Text == tree2.Nodes[0].Text)
            {
                TreeNode n1 = tree1.Nodes[0], n2 = tree2.Nodes[0];
                while (n1.Nodes.Count > 0 && n2.Nodes.Count > 0)
                {
                    bool found = false;
                    for (int i = 0; i < n1.Nodes.Count; i++)
                    {
                        if(n1.Nodes[i].Text == n2.Nodes[0].Text)
                        {
                            found = true;
                            n1 = n1.Nodes[i];
                            n2 = n2.Nodes[0];
                            break;
                        }
                    }
                    if(!found)
                    {
                        n1.Nodes.Add(n2.Nodes[0]);
                        break;
                    }
                }
                return tree1;
            }
            return null;
        }
        public TreeNode GetAppTree()
        {
            return appTree;
        }
    }
}
