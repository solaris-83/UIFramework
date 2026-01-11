using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using UIFrameworkDotNet;
using UIFrameworkDotNet.Helpers;

namespace UnitTestProject1
{
    [TestClass]
    public class PageTests
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
            libraryUI.ShowAndWait(menuPage);

            var chbx4 = menuPage.AddItem("NewCheckbox", "New Checkbox");
            // Assert
            var numSections = menuPage.FindAllByType<UISection>();
            Assert.IsTrue(numSections.Count() == 1);

            var numTabs = menuPage.FindAllByType<UITab>();
            Assert.IsTrue(numTabs.Count() == 1);

            var numTabControls = menuPage.FindAllByType<UITabControl>();
            Assert.IsTrue(numTabControls.Count() == 1);

            // Assert
            Assert.IsTrue(menuPage.HasCheckboxes);
            Assert.IsFalse(menuPage.IsMultipleSelection);
            Assert.AreEqual("", menuPage.SelectedId);
            Assert.AreEqual(-1, menuPage.SelectedIndex);
            Assert.IsFalse(menuPage.SelectedIds.ContainsAny("Activate_Injectors"));
            Assert.IsFalse(menuPage.SelectedIndexes.ContainsAny(0));

            // Act - simulate JS checking the first checkbox
            libraryUI.SimulateJsEvent(chbx1.Id, "propertyChanged",
                new Dictionary<string, object> { ["checked"] = true });

            // Assert
            Assert.IsTrue(menuPage.HasCheckboxes);
            Assert.IsFalse(menuPage.IsMultipleSelection);
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
            libraryUI.ShowAndWait(menuPage);

            var chbx4 = menuPage.AddItem("NewCheckbox", "New Checkbox"); // index 3

            // Assert
            Assert.IsTrue(menuPage.HasCheckboxes);
            Assert.IsTrue(menuPage.IsMultipleSelection);
            Assert.ThrowsException<InvalidOperationException>(() => menuPage.SelectedId);
            Assert.ThrowsException<InvalidOperationException>(() => menuPage.SelectedIndex);
            Assert.IsFalse(menuPage.SelectedIds.ContainsAny("Activate_Injectors"));
            Assert.IsFalse(menuPage.SelectedIndexes.ContainsAny(0));

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
            Assert.IsTrue(menuPage.SelectedIds.ContainsAny("Activate_Injectors", "Activate_Coils", "NewCheckbox"));
            Assert.IsTrue(menuPage.SelectedIndexes.ContainsAny(0, 1, 2));

            var numSections = menuPage.FindAllByType<UISection>();
            Assert.IsTrue(numSections.Count() == 1);

            var numTabs = menuPage.FindAllByType<UITab>();
            Assert.IsTrue(numTabs.Count() == 1);

            var numTabControls = menuPage.FindAllByType<UITabControl>();
            Assert.IsTrue(numTabControls.Count() == 1);
        }

        [TestMethod]
        public void CreatePageDisclaimer_Test()
        {
            // ==== DISCLAIMER PAGE ====
            // Istanzio disclaimer che crea 2 buttons EXIT_WITHOUT_REPORT e CONTINUE e un tab
            var libraryUI = new LibraryUI();
            var page = libraryUI.CreatePageDisclaimer();
            var par1 = page.AddParagraph(FakeStrings.PAR1, "paragraph", "gray");
            var par2 = page.AddParagraph(FakeStrings.PAR2, "paragraph", "gray");
            var item1 = page.AddBulletedItem(FakeStrings.ITEM_BULLETED_1);
            var item2 = page.AddBulletedItem(FakeStrings.ITEM_BULLETED_2);
            var item3 = page.AddOrderedItem(FakeStrings.ITEM_1);
            var item4 = page.AddOrderedItem(FakeStrings.ITEM_2);
            var item5 = page.AddOrderedItem(FakeStrings.ITEM_3);
            var btn = page.AddButton("CLICK ON ME!!", true);
            var par1Updated = page.UpdateParagraph(par1.Id, FakeStrings.PAR1_UPDATED);
            page.Remove(par2.Id);
            var img = page.AddImage("Screenshot.png");
            libraryUI.ShowAndWait(page);

            var fakeTranslations = new FakeTranslations();
            // Assert
            UIElement el = page.FindById(par1.Id);
            Assert.IsTrue(el != null);
            Assert.IsTrue(el is UILabel);
            var label = (UILabel)el;
            Assert.AreEqual(fakeTranslations.GetLocalOrDefault(FakeStrings.PAR1_UPDATED), label.Text);

            var numSections = page.FindAllByType<UISection>();
            Assert.IsTrue(numSections.Count() == 1);

            var numTabs = page.FindAllByType<UITab>();
            Assert.IsTrue(numTabs.Count() == 1);

            var numTabControls = page.FindAllByType<UITabControl>();
            Assert.IsTrue(numTabControls.Count() == 1);

            el = page.FindById(par2.Id);
            Assert.IsTrue(el == null);

            el = page.FindById(item1.Id);
            Assert.IsTrue(el != null);
            Assert.IsTrue(el is UILabel);
            label = (UILabel)el;
            Assert.AreEqual(fakeTranslations.GetLocalOrDefault(FakeStrings.ITEM_BULLETED_1), label.Text);

            el = page.FindById(item2.Id);
            Assert.IsTrue(el != null);
            Assert.IsTrue(el is UILabel);
            label = (UILabel)el;
            Assert.AreEqual(fakeTranslations.GetLocalOrDefault(FakeStrings.ITEM_BULLETED_2), label.Text);

            el = page.FindById(item3.Id);
            Assert.IsTrue(el != null);
            Assert.IsTrue(el is UILabel);
            label = (UILabel)el;
            Assert.AreEqual(fakeTranslations.GetLocalOrDefault(FakeStrings.ITEM_1), label.Text);

            el = page.FindById(item4.Id);
            Assert.IsTrue(el != null);
            Assert.IsTrue(el is UILabel);
            label = (UILabel)el;
            Assert.AreEqual(fakeTranslations.GetLocalOrDefault(FakeStrings.ITEM_2), label.Text);

            el = page.FindById(item5.Id);
            Assert.IsTrue(el != null);
            Assert.IsTrue(el is UILabel);
            label = (UILabel)el;
            Assert.AreEqual(fakeTranslations.GetLocalOrDefault(FakeStrings.ITEM_3), label.Text);

            el = page.FindById(btn.Id);
            Assert.IsTrue(el != null);
            Assert.IsTrue(el is UIButton);
            var button = (UIButton)el;
            Assert.AreEqual("CLICK ON ME!!", button.Text);
            Assert.AreEqual(true, button.Enabled);

            el = page.FindById(img.Id);
            Assert.IsTrue(el != null);
            Assert.IsTrue(el is UIImage);
            var image = (UIImage)el;
            Assert.AreNotEqual("", image.Source);


            libraryUI.SimulateJsEvent(btn.Id, "propertyChanged",
                    new Dictionary<string, object> { ["enabled"] = false });

            el = page.FindById(btn.Id);
            Assert.IsTrue(el != null);
            Assert.IsTrue(el is UIButton);
            button = (UIButton)el;
            Assert.AreEqual("CLICK ON ME!!", button.Text);
            Assert.AreEqual(false, button.Enabled);

            var par3 = page.AddParagraph("Paragraph #3 after ShowAndWait");

            el = page.FindById(par3.Id);
            Assert.IsTrue(el != null);
            Assert.IsTrue(el is UILabel);
            label = (UILabel)el;
            Assert.AreEqual("Paragraph #3 after ShowAndWait", label.Text);

            page.UpdateParagraph(par3.Id, "Paragraph #3 - BIS -  after ShowAndWait");

            el = page.FindById(par3.Id);
            Assert.IsTrue(el != null);
            Assert.IsTrue(el is UILabel);
            label = (UILabel)el;
            Assert.AreEqual("Paragraph #3 - BIS -  after ShowAndWait", label.Text);
        }

        [TestMethod]

        public void Page_UITab_UISection_Composition()
        {
            LibraryUI libraryUI = new LibraryUI();
            var page = libraryUI.CreatePage();
            var myTab = page.AddTab("tab", 2, 2);
            var mySection1 = libraryUI.CreateSectionDocument();
            // prevedo una section 1x1 visto che non specifico nient'altro
            mySection1.AddParagraph("Sono un paragraph");
            var mySection2 = libraryUI.CreateSectionDocument(); // prevedo una section 1x1 visto che non specifico nient'altro
            mySection2.AddImage("Screenshot.png");
            var mySection3 = libraryUI.CreateSectionDocument(); // prevedo una section 1x1 visto che non specifico nient'altro mySection3.AddChart()
            myTab.Add(mySection1, 0, 0); // (quadrante in alto sx)
            myTab.Add(mySection2, 0, 1); // (quadranti superiori)
            mySection3.GridPosition.ColumnSpan = 2;
            mySection3.Grid.Rows = 1;
            mySection3.Grid.Columns = 3;
            myTab.Add(mySection3, 1, 0); // mySection3 è posizionato alla riga di indice 1 e colonna di indice 0 per un Colspan pari a 2

            libraryUI.ShowAndWait(page);

            var numSections = page.FindAllByType<UISection>();
            Assert.IsTrue(numSections.Count() == 3);

            var numTabs = page.FindAllByType<UITab>();
            Assert.IsTrue(numTabs.Count() == 1);

            var numTabControls = page.FindAllByType<UITabControl>();
            Assert.IsTrue(numTabControls.Count() == 1);
        }

        [TestMethod]
        public void PageDisclaimer_Paragraph_UpdatedByJs()
        {
            var libraryUI = new LibraryUI();
            var page = libraryUI.CreatePageDisclaimer();
            var par1 = page.AddParagraph(FakeStrings.PAR1, "paragraph", "gray");
            libraryUI.ShowAndWait(page);
            libraryUI.SimulateJsEvent(par1.Id, "propertyChanged", new Dictionary<string, object> { ["text"] = "__NOTFOUND__" });

            var el = page.FindById(par1.Id);
            Assert.IsTrue(el != null);
            Assert.IsTrue(el is UILabel);
            var label = (UILabel)el;
            Assert.AreEqual("__NOTFOUND__", label.Text);

        }
    }
}
