namespace SciterCore
{
    public struct SciterSize
    {
        public static readonly SciterSize Empty = new SciterSize();

        private int _width;
        private int _height;
        
        /**
         * Create a new Size object of the specified dimension
         */
        /// <summary>
        ///    Initializes a new instance of the <see cref='SciterCore.SciterSize'/> class from
        ///    the specified dimensions.
        /// </summary>
        public SciterSize(int width, int height)
        {
            _width = width;
            _height = height;
        }
        
        /// <summary>
        ///    <para>
        ///       Represents the horizontal component of this
        ///    <see cref='SciterCore.SciterSize'/>.
        ///    </para>
        /// </summary>
        public int Width
        {
            get => _width;
            set => _width = value;
        }

        /**
         * Vertical dimension
         */

        /// <summary>
        ///    Represents the vertical component of this
        /// <see cref='SciterCore.SciterSize'/>.
        /// </summary>
        public int Height
        {
            get => _height;
            set => _height = value;
        }

        public override string ToString()
        {
            return $"{Width},{Height}";
        }
    }
}