namespace SciterCore
{
    public enum StringConversionType : int
    {
        /// <summary>
        /// <para>CVT_SIMPLE</para>
        /// Simple conversion of terminal values.
        /// </summary>
        Simple,
        
        /// <summary>
        /// <para>CVT_JSON_LITERAL</para>
        /// JSON literal parsing/emission.
        /// </summary>
        JsonLiteral,
        
        /// <summary>
        /// <para>CVT_JSON_MAP</para>
        /// JSON parsing/emission, it parses as if token '{' already recognized.
        /// </summary>
        JsonMap,
        
        /// <summary>
        /// <para>CVT_XJSON_LITERAL</para>
        /// <para>X-JSON parsing/emission.<br/>
        /// Date is emitted as ISO8601 date literal.<br/>
        /// Currency is emitted in the form DDDD$CCC</para>
        /// </summary>
        XJsonLiteral,
    }
}