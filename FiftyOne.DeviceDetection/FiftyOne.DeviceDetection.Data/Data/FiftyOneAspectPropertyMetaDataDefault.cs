/* *********************************************************************
 * This Original Work is copyright of 51 Degrees Mobile Experts Limited.
 * Copyright 2022 51 Degrees Mobile Experts Limited, Davidson House,
 * Forbury Square, Reading, Berkshire, United Kingdom RG1 3EU.
 *
 * This Original Work is licensed under the European Union Public Licence
 * (EUPL) v.1.2 and is subject to its terms as set out below.
 *
 * If a copy of the EUPL was not distributed with this file, You can obtain
 * one at https://opensource.org/licenses/EUPL-1.2.
 *
 * The 'Compatible Licences' set out in the Appendix to the EUPL (as may be
 * amended by the European Commission) shall be deemed incompatible for
 * the purposes of the Work and the provisions of the compatibility
 * clause in Article 5 of the EUPL shall not apply.
 * 
 * If using the Work as, or as part of, a network application, by 
 * including the attribution notice(s) required under Article 5 of the EUPL
 * in the end user terms of the application under an appropriate heading, 
 * such notice(s) shall fulfill the requirements of that article.
 * ********************************************************************* */

using FiftyOne.Pipeline.Engines.Data;
using FiftyOne.Pipeline.Engines.FiftyOne.Data;
using FiftyOne.Pipeline.Engines.FlowElements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FiftyOne.DeviceDetection.Shared.Data
{
    /// <summary>
    /// Data class that contains meta-data relating to a specific 
    /// property. 
    /// See the <see href="https://github.com/51Degrees/specifications/blob/main/data-model-specification/README.md#property">Specification</see>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design",
        "CA1036:Override methods on comparable types",
        Justification = "Mathematical operator-style methods are not " +
        "appropriate for this class.")]
    public abstract class FiftyOneAspectPropertyMetaDataDefault : 
        AspectPropertyMetaData, IFiftyOneAspectPropertyMetaData
    {
        private ComponentMetaDataDefault _component;
        private ValueMetaDataDefault _defaultValue;
        private List<IValueMetaData> _values;

        /// <summary>
        /// The component associated with this property.
        /// </summary>
        [Obsolete("This property is obsolete. Use the 'GetComponent' " +
            "method instead")]
        public IComponentMetaData Component => GetComponent();
        /// <summary>
        /// Get the default value for this property
        /// </summary>
        [Obsolete("This property is obsolete. Use the 'GetDefaultValue' " +
            "method instead")]
        public virtual IValueMetaData DefaultValue => GetDefaultValue();
        /// <summary>
        /// A byte used to order property meta-data instances. For example
        /// when displaying a list of properties to a user.
        /// </summary>
        public byte DisplayOrder { get; }
        /// <summary>
        /// True if this is a 'list' property. False if not.
        /// List properties can have multiple values on a single profile.
        /// For example, an Apple device such as the iPhone 8 has multiple 
        /// model numbers - A1863, A1864, A1897, etc. Therefore the 
        /// HardwareModelVariants property is a list property.
        /// </summary>
        public bool List { get; }
        /// <summary>
        /// True if this property must be filled in. False if it can be 
        /// left blank or 'Unknown'
        /// </summary>
        public bool Mandatory { get; }
        /// <summary>
        /// True if the property is obsolete. False otherwise.
        /// Obsolete properties are usually retained for some time for
        /// backwards compatibility but may be removed in a future release 
        /// and should not be used if possible.
        /// </summary>
        public bool Obsolete { get; }
        /// <summary>
        /// True if 51Degrees recommends that the property should appear 
        /// in lists shown to the user. False if not.
        /// </summary>
        /// <remarks>
        /// This is used by the 51Degrees Property Dictionary: 
        /// https://51degrees.com/resources/property-dictionary and is 
        /// made available for customers should they wish to make use of it.
        /// </remarks>
        public bool Show { get; }
        /// <summary>
        /// True if 51Degrees recommends that all possible values of this 
        /// property can be shown to the user. False if not.
        /// Properties that have very large lists of possible values will
        /// usually have this set to false.
        /// </summary>
        /// <remarks>
        /// This is used by the 51Degrees Property Dictionary: 
        /// https://51degrees.com/resources/property-dictionary and is 
        /// made available for customers should they wish to make use of it.
        /// </remarks>
        public bool ShowValues { get; }
        /// <summary>
        /// A URL that will display a page with more detail about the 
        /// property.
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// Get a list of meta-data objects for all the possible values 
        /// that exist for this property in the current data file.
        /// Note that as this instance is for meta-data that is generated
        /// from the c# code rather than the data file, this list will 
        /// only even contain the default value and that many other values
        /// are possible.
        /// </summary>
        [Obsolete("This property is obsolete. Use the 'GetValues' " +
            "method instead")]
        public virtual IReadOnlyList<IValueMetaData> Values => GetValues().ToList();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="category"></param>
        /// <param name="dataTiersWherePresent"></param>
        /// <param name="available"></param>
        /// <param name="component"></param>
        /// <param name="defaultValue"></param>
        /// <param name="values"></param>
        /// <param name="description"></param>
        public FiftyOneAspectPropertyMetaDataDefault(
            IAspectEngine element,
            string name,
            Type type,
            string category,
            IList<string> dataTiersWherePresent,
            bool available,
            ComponentMetaDataDefault component,
            ValueMetaDataDefault defaultValue,
            IEnumerable<ValueMetaDataDefault> values,
            string description)
            : this(element,
                name,
                type,
                category,
                dataTiersWherePresent,
                available,
                component,
                defaultValue,
                values,
                description,
                255,
                false, false, false, true, true, string.Empty)
        { }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="element"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="category"></param>
        /// <param name="dataTiersWherePresent"></param>
        /// <param name="available"></param>
        /// <param name="component"></param>
        /// <param name="defaultValue"></param>
        /// <param name="values"></param>
        /// <param name="description"></param>
        /// <param name="displayOrder"></param>
        /// <param name="list"></param>
        /// <param name="mandatory"></param>
        /// <param name="obsolete"></param>
        /// <param name="show"></param>
        /// <param name="showValues"></param>
        /// <param name="url"></param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if a required parameter is null
        /// </exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", 
            "CA1054:Uri parameters should not be strings", 
            Justification = "This would be a breaking change that we " +
            "will not be making at this time")]
        public FiftyOneAspectPropertyMetaDataDefault(
            IAspectEngine element,
            string name,
            Type type,
            string category,
            IList<string> dataTiersWherePresent,
            bool available,
            ComponentMetaDataDefault component,
            ValueMetaDataDefault defaultValue,
            IEnumerable<ValueMetaDataDefault> values,
            string description,
            byte displayOrder,
            bool list,
            bool mandatory,
            bool obsolete,
            bool show,
            bool showValues,
            string url) : base(element, name, type, category, dataTiersWherePresent, available, description)
        {
            if (component == null) { throw new ArgumentNullException(nameof(component)); }
            if (defaultValue == null) { throw new ArgumentNullException(nameof(defaultValue)); }

            _component = component;
            _defaultValue = defaultValue;
            DisplayOrder = displayOrder;
            List = list;
            Mandatory = mandatory;
            Obsolete = obsolete;
            Show = show;
            ShowValues = showValues;
            Url = url;

            // Add this property to the configured component.
            _component.AddProperty(this);
            // Set the property of the default value to this.
            _defaultValue.SetProperty(this);
            _values = values.Select(v =>
            {
                v.SetProperty(this);
                return (IValueMetaData)v;
            }).ToList();
        }

        /// <summary>
        /// Get the meta-data for the component associated with this property.
        /// The component is the major logical grouping to which this 
        /// property belongs. For example 'hardware device' or 'web browser'.
        /// </summary>
        /// <returns>
        /// An <see cref="IComponentMetaData"/> instance containing the 
        /// details for the component this property belongs to.
        /// </returns>
        public IComponentMetaData GetComponent()
        {
            return _component;
        }

        /// <summary>
        /// Get the meta-data for the default value for this property.
        /// </summary>
        /// <returns>
        /// An <see cref="IValueMetaData"/> instance containing the 
        /// details for the default value for this property.
        /// </returns>
        public IValueMetaData GetDefaultValue()
        {
            return _defaultValue;
        }

        /// <summary>
        /// Get the meta-data for the specified value for this property.
        /// </summary>
        /// <param name="valueName">
        /// The name of the value to get the meta-data for.
        /// </param>
        /// <returns>
        /// An <see cref="IValueMetaData"/> instance containing the 
        /// details for the specified value for this property.
        /// Null if the specified value was not found.
        /// </returns>
        public IValueMetaData GetValue(string valueName)
        {
            return GetValues().SingleOrDefault(v => v.Name == valueName);
        }

        /// <summary>
        /// Get a list of meta-data objects for all the possible values 
        /// that exist for this property in the current data file.
        /// Note that as this instance is for meta-data that is generated
        /// from the c# code rather than the data file, this list will 
        /// only even contain the default value and that many other values
        /// are possible.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public IEnumerable<IValueMetaData> GetValues()
        {
            return _values;
        }

        /// <summary>
        /// Compare this instance to another object that implements
        /// <see cref="IFiftyOneAspectPropertyMetaData"/>.
        /// </summary>
        /// <param name="other">
        /// The object to compare to.
        /// </param>
        /// <returns>
        /// &gt;0 if this instance precedes `other` in the sort order.
        /// 0 if they are equal in the sort order.
        /// &lt;0 if `other` precedes this instance in the sort order.
        /// </returns>
        public int CompareTo(IFiftyOneAspectPropertyMetaData other)
        {
            if (other == null) return -1;
            return string.Compare(Name, other.Name, 
                StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Check if this instance is equal to another object that 
        /// implements <see cref="IFiftyOneAspectPropertyMetaData"/>.
        /// </summary>
        /// <param name="other">
        /// The <see cref="IFiftyOneAspectPropertyMetaData"/> to check for 
        /// equality
        /// </param>
        /// <returns>
        /// True if the two instances are equal.
        /// False otherwise
        /// </returns>
        public bool Equals(IFiftyOneAspectPropertyMetaData other)
        {
            if (other == null) return false;
            return Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Check if this instance is equal to the supplied string.
        /// This uses the Name property to check equality.
        /// </summary>
        /// <param name="other">
        /// The <see cref="string"/> to check for equality.
        /// </param>
        /// <returns>
        /// True if the two instances are equal.
        /// False otherwise
        /// </returns>
        public bool Equals(string other)
        {
            return Name.Equals(other, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Get the hash code for this instance.
        /// </summary>
        /// <returns>
        /// The hash code.
        /// </returns>
        public override int GetHashCode()
        {
            return Name.ToUpperInvariant().GetHashCode();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing">
        /// False if being called from finalizer.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _component?.Dispose();
                    _defaultValue?.Dispose();
                }
                disposedValue = true;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}