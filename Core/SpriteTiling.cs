using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Maze
{
    /// <summary>
    /// Tiles often look better if they are drawn with different parts based on their connections. This is determined by
    /// the tiling system in use.
    /// </summary>
    public enum TilingSystem
    {
        /// <summary>
        /// The tile does not respond to any kind of connections.
        /// </summary>
        None,

        /// <summary>
        /// The tile is drawn in parts such that it visually connects to itself.
        /// 
        /// This system is a tileset of up to 5 tiles: isolated, corners, horizontal, vertical, and filled. Each of
        /// these five tile drawings is divisible in quarters; each tile is drawn as a combination of these quarter
        /// tiles. This can effectively represent any four-directional combination of tiles.
        /// </summary>
        FiveTile,
    }

    /// <summary>
    /// Deals with representing connections in tiles.
    /// Affects all variables of rectSrc and rectDst.
    /// </summary>
    public class SpriteTiling
    {
        /// <summary>
        /// Each bit represents, in clockwise order from top-left to bottom-right, whether the tile connects (1)
        /// or not (0). This configuration is used with the tile system to determine which tile to draw.
        /// </summary>
        public byte tileConfiguration = 0b00111110;

        /// <summary>
        /// Sprites with matching tile slots are considered connectible.
        /// </summary>
        public int[] tileSlots;

        /// <summary>
        /// If the tile system isn't none, this works with the tile configuration to perform the display of the sprite.
        /// </summary>
        public TilingSystem tileSystem;

        /// <summary>
        /// The underlying sprite, whose source and destination rectangles are used as the basis for any changes made by
        /// the tiling system.
        /// </summary>
        public Sprite sprite;

        /// <summary>
        /// Animates through the given sprite's frames. drawBehavior is set
        /// to be at least basicAnimated.
        /// </summary>
        /// <param name="sprite">
        /// The sprite to use (must have a texture defined).
        /// </param>
        public SpriteTiling(Sprite sprite)
        {
            this.sprite = sprite;

            /// <see cref="SpriteDraw.BasicAnimated"/> or better is required to function.
            if (this.sprite.drawBehavior == SpriteDraw.Basic)
            {
                this.sprite.drawBehavior = SpriteDraw.BasicAnimated;
            }

            sprite.rectSrc.Width = frameWidth;
            sprite.rectSrc.Height = frameHeight;
            sprite.rectDest.Width = frameWidth * this.sprite.scaleX;
            sprite.rectDest.Height = frameHeight * this.sprite.scaleY;
        }
        
        /// <summary>
        /// Animates through the given sprite's frames. drawBehavior is set
        /// to be at least basicAnimated.
        /// </summary>
        /// <param name="sprite">
        /// The sprite to use (must have a texture defined).
        /// </param>
        /// <param name="frameWidth">
        /// The width of each frame in the texture.
        /// </param>
        /// <param name="frameHeight">
        /// The height of each frame in the texture.
        /// </param>
        /// <param name="frames">
        /// The number of total frames.
        /// </param>
        /// <param name="rows">
        /// The number of rows (vertical frames) in the texture.
        /// </param>
        /// <param name="cols">
        /// The number of columns (side-by-side frames) in the texture.
        /// </param>
        public SpriteAtlas(Sprite sprite, int frameWidth, int frameHeight, int frames, int rows, int cols)
        {
            this.sprite = sprite;

            //basicAnimated or Animated are required to function.
            if (this.sprite.drawBehavior == SpriteDraw.Basic)
            {
                this.sprite.drawBehavior = SpriteDraw.BasicAnimated;
            }

            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.frames = frames;
            atlasRows = rows;
            atlasCols = cols;
            sprite.rectSrc.Width = frameWidth;
            sprite.rectSrc.Height = frameHeight;
            sprite.rectDest.Width = frameWidth * this.sprite.scaleX;
            sprite.rectDest.Height = frameHeight * this.sprite.scaleY;
        }

        /// <summary>
        /// Animates through the given sprite's frames. drawBehavior is set
        /// to be at least basicAnimated.
        /// </summary>
        /// <param name="sprite">
        /// The sprite to use (must have a texture defined).
        /// </param>
        /// <param name="frameWidth">
        /// The width of each frame in the texture.
        /// </param>
        /// <param name="frameHeight">
        /// The height of each frame in the texture.
        /// </param>
        /// <param name="frames">
        /// The number of total frames.
        /// </param>
        /// <param name="rows">
        /// The number of rows (vertical frames) in the texture.
        /// </param>
        /// <param name="cols">
        /// The number of columns (side-by-side frames) in the texture.
        /// </param>
        /// <param name="frameSpeed">
        /// Sets the frame update speed (how many update calls required per
        /// frame switch). Can be negative to cycle backwards.
        /// </param>
        public SpriteAtlas(Sprite sprite, int frameWidth, int frameHeight, int frames, int rows, int cols, double frameSpeed)
        {
            this.sprite = sprite;

            //basicAnimated or Animated are required to function.
            if (this.sprite.drawBehavior == SpriteDraw.Basic)
            {
                this.sprite.drawBehavior = SpriteDraw.BasicAnimated;
            }

            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.frames = frames;
            atlasRows = rows;
            atlasCols = cols;
            this.frameSpeed = frameSpeed;
            sprite.rectSrc.Width = frameWidth;
            sprite.rectSrc.Height = frameHeight;
            sprite.rectDest.Width = frameWidth * this.sprite.scaleX;
            sprite.rectDest.Height = frameHeight * this.sprite.scaleY;
        }

        /// <summary>
        /// All values are copied except for the sprite, which is referenced.
        /// </summary>
        public SpriteAtlas(SpriteAtlas atlas)
        {
            sprite = atlas.sprite;
            frame = atlas.frame;
            frames = atlas.frames;
            frameSpeed = atlas.frameSpeed;
            frameEndBehavior = atlas.frameEndBehavior;
            frameWidth = atlas.frameWidth;
            frameHeight = atlas.frameHeight;
            atlasRows = atlas.atlasRows;
            atlasCols = atlas.atlasCols;
            frameOffsetH = atlas.frameOffsetH;
            frameOffsetV = atlas.frameOffsetV;
        }

        /// <summary>
        /// Initializes a sprite atlas from another.
        /// </summary>
        /// <param name="spriteAtlas">The existing sprite atlas.</param>
        /// <param name="sameFrame">
        /// Whether the frame is copied or set to 0.
        /// </param>
        public SpriteAtlas(SpriteAtlas spriteAtlas, bool sameFrame)
        {
            atlasCols = spriteAtlas.atlasCols;
            atlasRows = spriteAtlas.atlasRows;
            frameEndBehavior = spriteAtlas.frameEndBehavior;
            frameHeight = spriteAtlas.frameHeight;
            frameOffsetH = spriteAtlas.frameOffsetH;
            frameOffsetV = spriteAtlas.frameOffsetV;
            frames = spriteAtlas.frames;
            frameSpeed = spriteAtlas.frameSpeed;
            frameWidth = spriteAtlas.frameWidth;
            if (sameFrame)
            {
                frame = spriteAtlas.frame;
            }

            sprite = spriteAtlas.sprite;
            sprite.rectSrc.Width = frameWidth;
            sprite.rectSrc.Height = frameHeight;
            sprite.rectDest.Width = frameWidth * sprite.scaleX;
            sprite.rectDest.Height = frameHeight * sprite.scaleY;
        }

        /// <summary>
        /// Centers the origin for the sprite atlas.
        /// </summary>
        public void CenterOrigin()
        {
            sprite.origin = new Vector2(frameWidth / 2, frameHeight / 2);
        }

        /// <summary>
        /// Sets the dimensions of the frames, how many there are, and the number of rows/cols.
        /// </summary>
        /// <param name="frameWidth">The width of each frame in the texture.</param>
        /// <param name="frameHeight">The height of each frame in the texture.</param>
        /// <param name="rows">The number of rows (vertical frames) in the texture.</param>
        /// <param name="cols">The number of columns (side-by-side frames) in the texture.</param>
        public void SetDimensions(int frameWidth, int frameHeight, int frames, int rows, int cols)
        {
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.frames = frames;
            atlasRows = rows;
            atlasCols = cols;
        }

        /// <summary>
        /// Sets the behavior of the animation at the end of the frame.
        /// Examples are to continue from the first frame, or reverse animation.
        /// </summary>
        /// <param name="endBehavior">The end behavior (see FrameEnd enum).</param>
        public void SetEndBehavior(FrameEnd endBehavior)
        {
            frameEndBehavior = endBehavior;
        }

        /// <summary>
        /// Sets the frame offsets, or spaces between each image.
        /// </summary>
        /// <param name="frameOffsetH">The number of horz. pixels between each image.</param>
        /// <param name="frameOffsetV">The number of vert pixels between each image.</param>
        public void SetOffsets(int frameOffsetH, int frameOffsetV)
        {
            this.frameOffsetH = frameOffsetH;
            this.frameOffsetV = frameOffsetV;
        }

        /// <summary>Updates the current frame if changed.</summary>
        /// <param name="doUpdateAlways">If false, update is only called when there are more than 0 frames and framespeed is not 0.</param>
        public void Update(bool doUpdateAlways)
        {
            if (!doUpdateAlways && (frames <= 0 || frameSpeed == 0))
            {
                return;
            }
            else
            {
                frame += frameSpeed;

                //If the frame actually changed
                if (((int)(frame - frameSpeed) != (int)frame))
                {
                    /* The following switch statement applies end behaviors.
                     * If the current frame is out of bounds (exceeding the
                     * number of frames or less than 0), then it will switch
                     * the frameSpeed or current frame to either stop the
                     * animation, reverse it, or loop from the beginning.
                    */

                    if ((int)frame >= frames || (int)frame < 0)
                    {
                        switch (frameEndBehavior)
                        {
                            case (FrameEnd.End):
                                frameSpeed = 0;
                                break;
                            case (FrameEnd.Loop):
                                if ((int)frame >= frames)
                                {
                                    frame = 0;
                                }
                                else
                                {
                                    frame = frames;
                                }
                                break;
                            case (FrameEnd.Reverse):
                                frameSpeed = -frameSpeed;
                                break;
                        }

                        OnAnimationEnd?.Invoke();
                    }
                }

                /*
                * Sets the actual positions of the subimages
                * This sets the x-value based on the number of frames, ignoring columns and rows.
                * The maximum x-value per y is computed and subtracted from the existing x-value
                * until it is under the max.  Every time it is subtracted, the column number is
                * increased.  Offsets are computed afterwards.
                */
                sprite.rectSrc.X = (int)frame * frameWidth;
                sprite.rectSrc.Y = 0;
                int maxWidth = atlasCols * frameWidth;
                int maxHeight = atlasRows * frameHeight;

                //Creates y wrapping
                while (sprite.rectSrc.X >= maxWidth)
                {
                    sprite.rectSrc.X -= maxWidth;
                    if (frame != frames)
                    {
                        sprite.rectSrc.Y += frameHeight;
                    }
                }

                //Calculates offsets
                sprite.rectSrc.X += frameOffsetH * (sprite.rectSrc.X / frameWidth);
                sprite.rectSrc.Y += frameOffsetV * (sprite.rectSrc.Y / frameHeight);
            }
        }
    }
}
