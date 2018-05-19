using System;

using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;

namespace WinSolution.Module {
    public class Updater : ModuleUpdater {
        public Updater(DevExpress.ExpressApp.IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();
            if (Session.FindObject<Level1>(null) == null) {
                Level1 l1 = new Level1(Session, "1");
                Level2 l2 = new Level2(Session, "2");
                Level3 l3 = new Level3(Session, "3");
                l1.Level2s.Add(l2);
                l2.Level3s.Add(l3);
                l1.Save();
                l2.Save();
                l3.Save();
                l1 = new Level1(Session, "1a");
                l2 = new Level2(Session, "2a");
                l3 = new Level3(Session, "3a");
                l1.Level2s.Add(l2);
                l2.Level3s.Add(l3);
                l1.Save();
                l2.Save();
                l3.Save();
            }
        }
    }
}
