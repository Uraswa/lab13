using System;
using System.Diagnostics.CodeAnalysis;
using Laba10;

namespace l10
{
    [ExcludeFromCodeCoverage]
    public class VRGame : VideoGame
    {

        public bool IsVRControllerRequired { get; private set; }
        public bool AreVRGlassesRequired { get; private set; }

        public string AreVRGlassesRequiredString => AreVRGlassesRequired ? "Да" : "Нет";
        public string IsVRControllerRequiredString => IsVRControllerRequired ? "Да" : "Нет";

        public VRGame() : base()
        {
            IsVRControllerRequired = false;
            AreVRGlassesRequired = false;
        }

        public VRGame(string name, uint minimumPlayers, uint maximumPlayers, IdNumber id, Device device, uint levelsCount, bool areVRGlassesRequired, bool isVRControllerRequired) : base(name, minimumPlayers, maximumPlayers, id, device, levelsCount)
        {
            IsVRControllerRequired = isVRControllerRequired;
            AreVRGlassesRequired = areVRGlassesRequired;
        }

        public VRGame(VRGame game) : base(game)
        {
            IsVRControllerRequired = game.IsVRControllerRequired;
            AreVRGlassesRequired = game.AreVRGlassesRequired;
        }

        public override string GetInfoAsString()
        {
            var baseString = base.GetInfoAsString();
            baseString += $"Нужны ли специальные очки?  {AreVRGlassesRequiredString}; ";
            baseString += $"Нужен ли специальный контроллер? {IsVRControllerRequiredString}; ";
            return baseString;
        }

        public new void Show()
        {
            Console.WriteLine($"" +
                $"Нужны ли специальные очки?  {AreVRGlassesRequiredString}; " +
                $"Нужен ли специальный контроллер? {IsVRControllerRequiredString}; "
                );
        }

        public override void ShowVirtual()
        {
            base.ShowVirtual();
            Show();
        }

        public override void Init()
        {
            base.Init();

            uint isVRContrRequired = Helpers.Helpers.EnterUInt("Нужен ли специальный контроллер для игры? 0 - нет, 1 - да", 0, 1);
            uint areGlassesRequired = Helpers.Helpers.EnterUInt("Нужны ли специальные очки для игры?  0 - нет, 1 - да", 0, 1);

            AreVRGlassesRequired = areGlassesRequired == 1;
            IsVRControllerRequired = isVRContrRequired == 1;
        }

        public override void RandomInit()
        {
            base.RandomInit();

            Random rnd = Helpers.Helpers.GetOrCreateRandom();

            int isVRContrRequired = rnd.Next(0, 2);
            int areGlassesRequired = rnd.Next(0, 2);

            AreVRGlassesRequired = areGlassesRequired == 1;
            IsVRControllerRequired = isVRContrRequired == 1;
        }

        protected override bool EqualsVirtual(object? obj)
        {
            bool parentEqual = base.EqualsVirtual(obj);
            return parentEqual
                && (obj is VRGame vR)
                && IsVRControllerRequired == vR.IsVRControllerRequired
                && AreVRGlassesRequired == vR.AreVRGlassesRequired;
        }
        
        public override int CompareTo(object? obj)
        {
            var resultCompare = base.CompareTo(obj);
            if (resultCompare != 0) return resultCompare;

            if (obj is not VRGame vrGame) return -1;

            if (AreVRGlassesRequired != vrGame.AreVRGlassesRequired)
            {
                return AreVRGlassesRequired.CompareTo(vrGame.AreVRGlassesRequired);
            }

            if (IsVRControllerRequired != vrGame.IsVRControllerRequired)
            {
                return IsVRControllerRequired.CompareTo(vrGame.IsVRControllerRequired);
            }

            return 0;
        }

        public override object Clone()
        {
            return new VRGame(Name, MinimumPlayers, MaximumPlayers, new IdNumber(Id.Number), Device, LevelCount, AreVRGlassesRequired, IsVRControllerRequired);

        }
    }
}
