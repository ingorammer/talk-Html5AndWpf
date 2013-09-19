using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TheIntegrator._0070_AdvancedCefSharp
{
    internal class TabPreloader
    {
        /// <summary>
        /// Preloads tab items of a tab control in sequence.
        /// </summary>
        /// <param name="tabControl">The tab control.</param>
        internal static void PreloadTabs(TabControl tabControl)
        {
            // Evaluate
            if (tabControl.Items != null)
            {
                // The first tab is already loaded
                // so, we will start from the second tab.
                if (tabControl.Items.Count > 1)
                {
                    // Hide tabs
                    tabControl.Opacity = 0.0;

                    // Last action
                    Action onComplete = () =>
                    {
                        // Set index to first tab
                        tabControl.SelectedIndex = 0;

                        // Show tabs
                        tabControl.Opacity = 1.0;
                    };

                    // Second tab
                    var firstTab = (tabControl.Items[1] as TabItem);
                    if (firstTab != null)
                    {
                        PreloadTab(tabControl, firstTab, onComplete);
                    }
                }
            }
        }

        /// <summary>
        /// Preloads an individual tab item.
        /// </summary>
        /// <param name="tabControl">The tab control.</param>
        /// <param name="tabItem">The tab item.</param>
        /// <param name="onComplete">The onComplete action.</param>
        private static void PreloadTab(TabControl tabControl, TabItem tabItem, Action onComplete = null)
        {
            // On update complete
            tabItem.Loaded += delegate
            {
                // Update if not the last tab
                if (tabItem != tabControl.Items[tabControl.Items.Count - 1])
                {
                    // Get next tab
                    var nextIndex = tabControl.Items.IndexOf(tabItem) + 1;
                    var nextTabItem = tabControl.Items[nextIndex] as TabItem;

                    // Preload
                    if (nextTabItem != null)
                    {
                        PreloadTab(tabControl, nextTabItem, onComplete);
                    }
                }

                else
                {
                    if (onComplete != null)
                    {
                        onComplete();
                    }
                }
            };

            // Set current tab context
            tabControl.SelectedItem = tabItem;
        }
    }
}
