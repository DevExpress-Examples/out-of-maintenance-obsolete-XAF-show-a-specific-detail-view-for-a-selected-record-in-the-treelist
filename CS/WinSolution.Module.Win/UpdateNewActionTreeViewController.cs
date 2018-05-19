using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Win.SystemModule;
using DevExpress.ExpressApp.Actions;

namespace WinSolution.Module.Win {
    public class UpdateNewActionTreeViewController : ViewController {
        private const string DefaultReason = "MyKey";
        private Type currentObjectType = null;
        public UpdateNewActionTreeViewController() {
            TargetObjectType = typeof(Category);
            TargetViewNesting = Nesting.Root;
        }
        protected override void OnActivated() {
            base.OnActivated();
            if (View is ListView) {
                ObjectSpace.Reloaded += new EventHandler(ObjectSpace_Reloaded);
                View.CurrentObjectChanged += new EventHandler(ListView_CurrentObjectChangedEventHandler);
                ((ListView)View).CreateCustomCurrentObjectDetailView += new EventHandler<CreateCustomCurrentObjectDetailViewEventArgs>(ListView_CreateCustomCurrentObjectDetailView);
            }
            else if (View is DetailView) {
                UpdateActionInDetailView();
                View.CurrentObjectChanged += new EventHandler(DetailView_CurrentObjectChangedEventHandler);
            }
        }
        protected override void OnDeactivating() {
            base.OnDeactivating();
            currentObjectType = null;
            if (View is ListView) {
                View.ObjectSpace.Reloaded -= new EventHandler(ObjectSpace_Reloaded);
                View.CurrentObjectChanged -= new EventHandler(ListView_CurrentObjectChangedEventHandler);
                ((ListView)View).CreateCustomCurrentObjectDetailView -= new EventHandler<CreateCustomCurrentObjectDetailViewEventArgs>(ListView_CreateCustomCurrentObjectDetailView);
            }
            if (View is DetailView) {
                View.CurrentObjectChanged -= new EventHandler(DetailView_CurrentObjectChangedEventHandler);
            }
        }
        void ObjectSpace_Reloaded(object sender, EventArgs e) {
            UpdateActionInListView();
        }
        void ListView_CurrentObjectChangedEventHandler(object sender, EventArgs e) {
            if (View.CurrentObject != null) {
                if (currentObjectType != View.CurrentObject.GetType()) {
                    currentObjectType = View.CurrentObject.GetType();
                    UpdateActionInListView();
                }
            }
            else {
                currentObjectType = null;
                UpdateActionInListView();
            }
        }
        void DetailView_CurrentObjectChangedEventHandler(object sender, EventArgs e) {
            UpdateActionInDetailView();
        }
        private void UpdateActionInDetailView() {
            if (View.CurrentObject == null) return;
            DevExpress.ExpressApp.Actions.SingleChoiceAction action = Frame.GetController<WinNewObjectViewController>().NewObjectAction;
            action.BeginUpdate();
            foreach (ChoiceActionItem item in action.Items) {
                item.Active.SetItemValue(DefaultReason, View.CurrentObject.GetType() == (Type)item.Data);
            }
            action.EndUpdate();
        }
        private void UpdateActionInListView() {
            DevExpress.ExpressApp.Actions.SingleChoiceAction action = Frame.GetController<WinNewObjectViewController>().NewObjectAction;
            action.BeginUpdate();
            foreach (ChoiceActionItem item in action.Items) {
                Type itemType = (Type)item.Data;

                item.Enabled.RemoveItem(DefaultReason);
                if ((itemType == typeof(Level1) || !typeof(Category).IsAssignableFrom(itemType)) && currentObjectType == null) {
                    continue;
                }
                if (itemType == typeof(Level2) && currentObjectType == typeof(Level1)) {
                    continue;
                }
                if (itemType == typeof(Level3) && currentObjectType == typeof(Level2)) {
                    continue;
                }
                item.Enabled.SetItemValue(DefaultReason, false);

                if (!typeof(Category).IsAssignableFrom(itemType)) {
                    item.Active.SetItemValue(DefaultReason, false);
                }
            }
            action.EndUpdate();
        }
        private void ListView_CreateCustomCurrentObjectDetailView(object sender, CreateCustomCurrentObjectDetailViewEventArgs e) {
            if (e.ListViewCurrentObject != null) {
                e.DetailViewId = Application.FindDetailViewId(e.ListViewCurrentObject.GetType());
            } else {
                e.DetailViewId = e.CurrentDetailView.Id;
            }
        }
    }
}