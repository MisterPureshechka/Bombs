namespace Bombs.Core.Borders
{
    public class SimpleBorder : BaseBorder
    {
        public override bool Block() => true;

        public override void Damage(float damage) { }
    }
}