using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using ICSharpCode.Decompiler;
using SciterCore.ILSpy.Languages;

namespace SciterCore.ILSpy.EventHandlers
{
    public abstract class RootEventHandler : SciterEventHandler
    {
        FilterSettings filterSettings;
        bool childrenNeedFiltering;

        public FilterSettings FilterSettings
        {
            get { return filterSettings; }
            set
            {
                if (filterSettings != value)
                {
                    filterSettings = value;
                    OnFilterSettingsChanged();
                }
            }
        }

        public virtual FilterResult Filter(FilterSettings settings)
        {
            if (string.IsNullOrEmpty(settings.SearchTerm))
                return FilterResult.Match;
            else
                return FilterResult.Hidden;
        }

        public Language Language => filterSettings != null ? filterSettings.Language : new CSharpLanguage() /*Languages.AllLanguages[0]*/;

        ///// <summary>
        ///// Used to implement special save logic for some items.
        ///// This method is called on the main thread when only a single item is selected.
        ///// If it returns false, normal decompilation is used to save the item.
        ///// </summary>
        //public virtual Task<bool> Save(TextView.DecompilerTextView textView)
        //{
        //    return Task.FromResult(false);
        //}

        //protected override void OnChildrenChanged(NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.NewItems != null)
        //    {
        //        if (IsVisible)
        //        {
        //            foreach (ILSpyTreeNode node in e.NewItems)
        //                ApplyFilterToChild(node);
        //        }
        //        else
        //        {
        //            childrenNeedFiltering = true;
        //        }
        //    }
        //    base.OnChildrenChanged(e);
        //}

        void ApplyFilterToChild(RootEventHandler child)
        {
            FilterResult r;
            if (this.FilterSettings == null)
                r = FilterResult.Match;
            else
                r = child.Filter(this.FilterSettings);
            switch (r)
            {
                case FilterResult.Hidden:
                    throw new NotImplementedException();
                    //child.IsHidden = true;
                    break;
                case FilterResult.Match:
                    child.FilterSettings = StripSearchTerm(this.FilterSettings);
                    throw new NotImplementedException();
                    //child.IsHidden = false;
                    break;
                case FilterResult.Recurse:
                    child.FilterSettings = this.FilterSettings;
                    child.EnsureChildrenFiltered();
                    throw new NotImplementedException();
                    //child.IsHidden = child.Children.All(c => c.IsHidden);
                    break;
                case FilterResult.MatchAndRecurse:
                    child.FilterSettings = StripSearchTerm(this.FilterSettings);
                    child.EnsureChildrenFiltered();
                    throw new NotImplementedException();
                    //child.IsHidden = child.Children.All(c => c.IsHidden);
                    break;
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        static FilterSettings StripSearchTerm(FilterSettings filterSettings)
        {
            if (filterSettings == null)
                return null;
            if (!string.IsNullOrEmpty(filterSettings.SearchTerm))
            {
                filterSettings = filterSettings.Clone();
                filterSettings.SearchTerm = null;
            }
            return filterSettings;
        }

        protected virtual void OnFilterSettingsChanged()
        {
            //RaisePropertyChanged("Text");
            //if (IsVisible)
            //{
            //    foreach (ILSpyTreeNode node in this.Children.OfType<ILSpyTreeNode>())
            //        ApplyFilterToChild(node);
            //}
            //else
            //{
            //    childrenNeedFiltering = true;
            //}
        }

        internal void EnsureChildrenFiltered()
        {
            //EnsureLazyChildren();
            //if (childrenNeedFiltering)
            //{
            //    childrenNeedFiltering = false;
            //    foreach (ILSpyTreeNode node in this.Children.OfType<ILSpyTreeNode>())
            //        ApplyFilterToChild(node);
            //}
        }

        public virtual bool IsPublicAPI
        {
            get { return true; }
        }

        public virtual bool IsAutoLoaded
        {
            get { return false; }
        }

    }

    public abstract class RootEventHandler<TParent, TValue> : RootEventHandler
    {

        protected readonly TParent Parent;
        protected readonly TValue Value;

        protected RootEventHandler(TParent parent, TValue value)
        {
            Parent = parent;
            Value = value;
        }

        
        public abstract void Decompile(Language language, ITextOutput output, DecompilationOptions options);

        public virtual bool DecompileOutput(SciterElement parent, SciterValue[] args, out SciterValue result)
        {
            //Language language = new CSharpLanguage();
            var output = new PlainTextOutput();
            Decompile(Language, output, new DecompilationOptions());

            result = new SciterValue($"<plaintext class='decompileview'>{output}</plaintext>");

            return true;
        }

        public abstract bool GetChildren(SciterElement parent, SciterValue[] args, out SciterValue result);

        protected abstract string Title { get; }

        protected abstract string Suffix { get; }

        protected abstract string Image { get; }

    }
}