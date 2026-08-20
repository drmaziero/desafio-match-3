using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Views.Onboarding
{
    public class OnboardingSpecialTileView : OnboardingElementView
    {
        [field: SerializeField] private Image[] normalTiles;
        [field: SerializeField] private Image specialTile;

        public override Tween Show()
        {
            var sequence = DOTween.Sequence();
            foreach (var tile in normalTiles)
                tile.color = new Color(tile.color.r, tile.color.g, tile.color.b, 0.0f);

            if (specialTile != null)
                specialTile.color = new Color(specialTile.color.r, specialTile.color.g, specialTile.color.b, 0.0f);

            foreach (var tile in normalTiles)
            {
                sequence.Join(DOTween.To(
                    () => tile.color.a,
                    alpha =>
                    {
                        Color color = tile.color;
                        color.a = alpha;
                        tile.color = color;
                    },
                    1.0f,
                    1.0f
                ));
            }

            sequence.AppendInterval(2f);
            bool firstFadeOut = true;
            
            foreach (var tile in normalTiles)
            {
                if (firstFadeOut)
                {
                    sequence.Append(DOTween.To(
                        () => tile.color.a,
                        alpha =>
                        {
                            Color color = tile.color;
                            color.a = alpha;
                            tile.color = color;
                        },
                        0.0f,
                        0.25f
                    ));

                    firstFadeOut = false;
                }
                else
                {
                    sequence.Join(DOTween.To(
                        () => tile.color.a,
                        alpha =>
                        {
                            Color color = tile.color;
                            color.a = alpha;
                            tile.color = color;
                        },
                        0.0f,
                        0.25f
                    ));
                }
            }

            if (specialTile != null)
            {
                sequence.Append(DOTween.To(
                    () => specialTile.color.a,
                    alpha =>
                    {
                        Color color = specialTile.color;
                        color.a = alpha;
                        specialTile.color = color;
                    },
                    1.0f,
                    1.0f
                ));
            }

            return sequence;
        }

        public override Tween Close()
        {
            var sequence = DOTween.Sequence();

            if (specialTile != null)
            {
                sequence.Append(DOTween.To(
                    () => specialTile.color.a,
                    alpha =>
                    {
                        Color color = specialTile.color;
                        color.a = alpha;
                        specialTile.color = color;
                    },
                    0.0f,
                    0.25f
                ));
            }

            return sequence;
        }
    }
}