namespace Maze
{
    /// <summary>
    /// Defines how a Sprite is drawn.
    /// </summary>
    public enum SpriteDraw
    {
        /// <summary>
        /// Draws a texture at the given position with minimal features, for performance. To draw with tile systems or
        /// animation, use <see cref="SpriteDraw.BasicAnimated"/> or <see cref="SpriteDraw.All"/> instead.
        /// </summary>
        Basic,

        /// <summary>
        /// Draws a texture at the given position and color blend, including source rectangle.
        /// </summary>
        BasicAnimated,
        
        /// <summary>
        /// Draws a texture at the given position with all possible features.
        /// </summary>
        All
    };
}
