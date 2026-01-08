using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class MenuPageTests
    {
        [TestMethod]
        public void SingleSelection_SelectsSingleItemAndUpdatesSelectedProperties()
        {
            // Arrange
            var libraryUI = new LibraryUI();
            var menuPage = libraryUI.CreatePageMenu();
            menuPage.HasCheckboxes = true;
            menuPage.IsMultipleSelection = false;
            menuPage.SetMessage("S_COMP");

            var chbx1 = menuPage.AddItem("Activate_Injectors", "Activte_Injectors");
            var chbx2 = menuPage.AddItem("Activate_Coils", "Activte_Ignition_Coil");
            var chbx3 = menuPage.AddItem("Deactivate_Injectors", "Deactivte_Injectors");

            // Act - simulate JS checking the first item
            libraryUI.SimulateJsEvent(chbx1.Id, "propertyChanged",
                new Dictionary<string, object> { ["checked"] = true });

            // Assert
            Assert.IsTrue(menuPage.HasCheckboxes);
            Assert.IsFalse(menuPage.IsMultipleSelection == true); // ensure single selection mode

            Assert.AreEqual("Activate_Injectors", menuPage.SelectedId);
            Assert.AreEqual(0, menuPage.SelectedIndex);

            Assert.IsTrue(menuPage.SelectedIds.ContainsAny("Activate_Injectors"));
            Assert.IsTrue(menuPage.SelectedIndexes.ContainsAny(0));
        }

        [TestMethod]
        public void MultipleSelection_AllowsMultipleCheckedItemsAndReportsCountsAndContainment()
        {
            // Arrange
            var libraryUI = new LibraryUI();
            var menuPage = libraryUI.CreatePageMenu();
            menuPage.HasCheckboxes = true;
            menuPage.IsMultipleSelection = true;
            menuPage.SetMessage("S_COMP");

            var chbx1 = menuPage.AddItem("Activate_Injectors", "Activte_Injectors");
            var chbx2 = menuPage.AddItem("Activate_Coils", "Activte_Ignition_Coil");
            var chbx3 = menuPage.AddItem("Deactivate_Injectors", "Deactivte_Injectors");
            var chbx4 = menuPage.AddItem("NewCheckbox", "New Checkbox"); // index 3

            // Act - simulate JS checking multiple items
            libraryUI.SimulateJsEvent(chbx1.Id, "propertyChanged",
                new Dictionary<string, object> { ["checked"] = true });
            libraryUI.SimulateJsEvent(chbx2.Id, "propertyChanged",
                new Dictionary<string, object> { ["checked"] = true });
            libraryUI.SimulateJsEvent(chbx4.Id, "propertyChanged",
                new Dictionary<string, object> { ["checked"] = true });

            // Assert
            Assert.IsTrue(menuPage.HasCheckboxes);
            Assert.IsTrue(menuPage.IsMultipleSelection);

            Assert.AreEqual(3, menuPage.SelectedIds.Count);
            Assert.AreEqual(3, menuPage.SelectedIndexes.Count);

            Assert.IsTrue(menuPage.SelectedIds.ContainsAny("Activate_Injectors", "Activate_Coils"));
            Assert.IsTrue(menuPage.SelectedIndexes.ContainsAny(0, 1));

            Assert.IsTrue(menuPage.SelectedIds.ContainsAny("NewCheckbox"));
            Assert.IsTrue(menuPage.SelectedIndexes.ContainsAny(3));
        }
    }
}
