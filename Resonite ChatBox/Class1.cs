using System;
using Elements.Core;
using FrooxEngine;
using FrooxEngine.UIX;
using HarmonyLib;
using ResoniteModLoader;

namespace Resonite_ChatBox {
    public class Class1 : ResoniteMod {
        public override string Name => "Resonite ChatBox";
        public override string Author => "HamoCorp";
        public override string Version => "1.0.0";

        private static ModConfiguration Config;

        public override void OnEngineInit() {

            Config = GetConfiguration();
            Config.Save(true);
            Harmony harmony = new Harmony("com.HamoCorp.ResoniteChatBox");
            harmony.PatchAll();
            Msg("ResoniteChatBox Mod is running");
        }

        public static string nameGenerator(int length) {

            string name = " ";
            for (int i = 0; i < length; i++) {
                name += " ";
            }
            return name;
        }
        private static void PogStreamPerams(ValueStream<string> stream) {

            stream.SetInterpolation();
            stream.SetUpdatePeriod(1, 0);
            stream.Encoding = ValueEncoding.Full;
            stream.FullFrameBits = 4;
        }
        private static void MakePogStreams() {

            
            Slot slot = _LocalUserRootSlot.AddSlot("<color=#ff0000>TestSlot", true);

            ValueStream<string> stream = _LocalUserRootSlot.LocalUser.GetStreamOrAdd<ValueStream<string>>("TestValue", PogStreamPerams);
            stream.Value = "abc";

            DynamicValueVariable<string> dyn = slot.AttachComponent<DynamicValueVariable<string>>(true, null);
            dyn.VariableName.Value = "User/" + "PogString";
            ValueDriver<string> vd = slot.AttachComponent<ValueDriver<string>>(true, null);

            vd.ValueSource.Target = stream;
            vd.DriveTarget.Target = dyn.Value;
        }
        private static void setStreamPerams(ValueStream<char> stream) {

            stream.SetInterpolation();
            stream.SetUpdatePeriod(1, 0);
            stream.Encoding = ValueEncoding.Full;
            stream.FullFrameBits = 4;
            //stream.FullFrameMin = '';
            //stream.FullFrameMax = "z";
        }
        private static void addChatBoxSlot() {

            

            _ChatText = "a";

            _ChatBoxSlot = _LocalUserRootSlot.AddSlot("ChatBox Mod", false);


            
            for (int i = 0; i < _VarSize; i++) {
                setupValueStream(i);
            }

            SetupContextMenu();
            SetupTextBoxVisual();

            

        }

        private static void setupValueStream(int i) {
            string str = "ligma ballz bro";
            _valueStream[i] = _LocalUserRootSlot.LocalUser.GetStreamOrAdd<ValueStream<char>>("ChatBox" + Convert.ToString(i), setStreamPerams);

            _valueStream[i].Value = str.ToCharArray()[i];// _ChatText;

            _dynamicValueText[i] = _ChatBoxSlot.AttachComponent<DynamicValueVariable<char>>(true, null);
            _dynamicValueText[i].VariableName.Value = "User/" + "com.HamoCorp.ResoniteChatBox." + Convert.ToString(i);
            _valueDriver[i] = _ChatBoxSlot.AttachComponent<ValueDriver<char>>(true, null);

            _valueDriver[i].ValueSource.Target = _valueStream[i];
            _valueDriver[i].DriveTarget.Target = _dynamicValueText[i].Value;
        }

        private static void SetupTextBoxVisual() {

            _TextBoxSlot = _ChatBoxSlot.AddSlot("Visual", true);
            _TextBoxSlot.AttachComponent<PositionAtUser>();
            
            _TextBoxStringSlot = _ChatBoxSlot.AddSlot("Text", true);
            _TextBoxStringSlot.AttachComponent<TextRenderer>();

        }

        private static void SetupContextMenu() {

            _ContextMenuRoot = _ChatBoxSlot.AddSlot("Context Menu", true);
            RootContextMenuItem rootcontext = _ContextMenuRoot.AttachComponent<RootContextMenuItem>();
            //    _ContextMenuRoot.AttachComponent<ContextMenuSubmenu>();
            ContextMenuItemSource cmitem = _ContextMenuRoot.AttachComponent<ContextMenuItemSource>();
 //           rootcontext.Item.Value = cmitem.ReferenceID;
            TextField tF = _ContextMenuRoot.AttachComponent<TextField>();
            TextEditor edit = _ContextMenuRoot.AttachComponent<TextEditor>();
            tF.Editor.Value = edit.ReferenceID;

            //    _ContextMenuRoot.AttachComponent<SpriteProvider>();
            

 //           Slot TextSlot = _ContextMenuRoot.AddSlot("Text", true);
 //           TextRenderer txt = TextSlot.AttachComponent<TextRenderer>();
 //           edit.Text.Value = txt.ReferenceID;
 //           PositionAtUser pu = TextSlot.AttachComponent<PositionAtUser>();
 //           pu.TargetUser.Value = _LocalUserRootSlot.ActiveUser.ReferenceID;
 //           pu.PositionSource.Value = UserRoot.UserNode.Head;
 //           pu.RotationSource.Value = UserRoot.UserNode.View;
 //           pu.PositionOffset.Value = new float3(0f, 0f, 1f);
            //    _ContextMenuNewMessage = _ContextMenuRoot.AddSlot("New Message");
            //    _ContextMenuNewMessage.AttachComponent<ContextMenuItemSource>();
            //    _ContextMenuNewMessage.AttachComponent<SpriteProvider>();
        }

        //class SlotData { public SlotData(int index) { this.index = index; } public int index; public Component[] components; }
        private enum CBComponents {
            FirstSlot = 1,
            TextBoxSlot = 2,
            TextBoxString = 3,

            ContextMenuRoot = 4,
            ContextMenuNewMessage = 5
        }

        private static string _ChatText;

        private static Slot _LocalUserRootSlot;

        private static Slot _ChatBoxSlot;
        private static Slot _TextBoxSlot;
        private static Slot _TextBoxStringSlot;
        private static Slot _ContextMenuRoot;
        private static Slot _ContextMenuNewMessage;

        private static Component _ChatBoxComponents;

        private static int _VarSize = 13;
        private static DynamicValueVariable<char>[] _dynamicValueText = new DynamicValueVariable<char>[_VarSize];
        private static ValueDriver<char>[] _valueDriver = new ValueDriver<char>[_VarSize];
        private static ValueStream<char>[] _valueStream = new ValueStream<char>[_VarSize];

        [HarmonyPatch]
        class ChatBoxPatch {

            [HarmonyPostfix]
            [HarmonyPatch(typeof(UserRoot), "OnStart")]
            public static void EditLocalUserRoot(UserRoot __instance) {

                if (__instance.Slot != null) {

                    if (__instance.Slot.ActiveUser != null && __instance.Slot.ActiveUser.IsLocalUser) {

                        if (__instance.Slot.Name.StartsWith("User")) {
                            _LocalUserRootSlot = __instance.Slot;
                            //MakePogStreams();
                            addChatBoxSlot();
                        }
                    }


                }
            }
        }

        

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<bool> _enabled = new ModConfigurationKey<bool>("enabled", "Enabled", () => true);

        [AutoRegisterConfigKey]
        public static readonly ModConfigurationKey<dummy> _d0 = new ModConfigurationKey<dummy>(nameGenerator(0), "");
        /*
            hide chatbox in avatar
            chatbox collor and outloine
            head or side with offset position
            enable voice
            hide context menu
            defult size

         */
        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<colorX> _service = new ModConfigurationKey<colorX>("Text Colour", "Text Color", () => new colorX(1,0,0,1));

        [AutoRegisterConfigKey]
        public static readonly ModConfigurationKey<string> _d1 = new ModConfigurationKey<string>("Pulsoid Key", "Enter your pulsoid Key", () => "");
    }


}

