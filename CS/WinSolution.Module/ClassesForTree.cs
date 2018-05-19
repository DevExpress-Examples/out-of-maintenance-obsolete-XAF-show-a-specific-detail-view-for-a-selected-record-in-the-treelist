using DevExpress.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using System.ComponentModel;
using DevExpress.Persistent.Base.General;

namespace WinSolution.Module {
    [NavigationItem]
    public abstract class Category : BaseObject, ITreeNode {
        private string name;
        protected abstract ITreeNode Parent {
            get;
        }
        protected abstract IBindingList Children {
            get;
        }
        public Category(Session session) : base(session) { }
        public string Name {
            get {
                return name;
            }
            set {
                name = value;
                SetPropertyValue("Name", ref name, value);
            }
        }
        #region ITreeNode
        IBindingList ITreeNode.Children {
            get {
                return Children;
            }
        }
        string ITreeNode.Name {
            get {
                return Name;
            }
        }
        ITreeNode ITreeNode.Parent {
            get {
                return Parent;
            }
        }
        #endregion
    }
    public class Level1 : Category {
        protected override ITreeNode Parent {
            get {
                return null;
            }
        }
        protected override IBindingList Children {
            get {
                return Level2s;
            }
        }
        public Level1(Session session) : base(session) { }
        public Level1(Session session, string name)
            : base(session) {
            this.Name = name;
        }
        [Association("Level1-Level2s"), Aggregated]
        public XPCollection<Level2> Level2s {
            get {
                return GetCollection<Level2>("Level2s");
            }
        }
    }
    public class Level2 : Category {
        private Level1 _level1;
        protected override ITreeNode Parent {
            get {
                return _level1;
            }
        }
        protected override IBindingList Children {
            get {
                return Level3s;
            }
        }
        public Level2(Session session) : base(session) { }
        public Level2(Session session, string name)
            : base(session) {
            this.Name = name;
        }
        [Association("Level1-Level2s")]
        public Level1 Level1 {
            get {
                return _level1;
            }
            set {
                SetPropertyValue("Level1", ref _level1, value);
            }
        }
        [Association("Level2-Level3s"), Aggregated]
        public XPCollection<Level3> Level3s {
            get {
                return GetCollection<Level3>("Level3s");
            }
        }
    }
    public class Level3 : Category {
        private Level2 _level2;
        protected override ITreeNode Parent {
            get {
                return _level2;
            }
        }
        protected override IBindingList Children {
            get {
                return new BindingList<object>();
            }
        }
        public Level3(Session session) : base(session) { }
        public Level3(Session session, string name)
            : base(session) {
            this.Name = name;
        }
        [Association("Level2-Level3s")]
        public Level2 Level2 {
            get {
                return _level2;
            }
            set {
                SetPropertyValue("Level2", ref _level2, value);
            }
        }
    }
}