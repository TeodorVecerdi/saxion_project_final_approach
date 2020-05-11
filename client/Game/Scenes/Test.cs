using System.Drawing;
using game.ui;
using game.utils;
using GXPEngine;
using Button = game.ui.Button;

namespace game {
    public class Test : Scene {
        public Test() {
            SceneName = "0";
        }
        
        public override void Load() {
            var primaryTitleStyle = new LabelStyle(Color.White, 20f, FontLoader.CenterCenterAlignment);
            var secondaryTitleStyle = new LabelStyle(Color.White, 16f, FontLoader.CenterCenterAlignment);
            var textFieldLabelStyle = new LabelStyle(Color.FromArgb(205, 205, 205), 14f, FontLoader.LeftCenterAlignment);

            var tab1 = new Pivot();
            var tab2 = new Pivot();
            tab2.SetXY(Globals.WIDTH/3f, 0f);
            var tab3 = new Pivot();
            tab3.SetXY(2f * Globals.WIDTH/3f, 0f);
            Root.AddChild(tab1);
            Root.AddChild(tab2);
            Root.AddChild(tab3);
            var tab1Main = new Pivot();
            var tab1CreatePublic = new Pivot() { x=-100000f};
            var tab1CreatePrivate = new Pivot() { x=-100000f};
            tab1.AddChild(tab1Main);
            tab1.AddChild(tab1CreatePublic);
            tab1.AddChild(tab1CreatePrivate);
            tab1Main.AddChild(new Label(0, 50, Globals.WIDTH/3f, 50, "CREATE ROOM", primaryTitleStyle));
            tab1Main.AddChild(new Button(40, 100, Globals.WIDTH/3f - 80, 40, "Public room", onClick: () => {
                tab1CreatePublic.x = 0;
                tab1Main.x = -100000f;
            }));
            tab1Main.AddChild(new Button(40, 150, Globals.WIDTH/3f - 80, 40, "Private room", onClick: () => {
                tab1CreatePrivate.x = 0;
                tab1Main.x = -100000f;
            }));
            tab1CreatePublic.AddChild(new Label(40, 0 + 40, Globals.WIDTH/3f - 80f, 40, "Create public room", primaryTitleStyle));
            tab1CreatePublic.AddChild(new Label(40, 0 + 40 + 60, Globals.WIDTH/3f-80, 24, "Room Name", textFieldLabelStyle));
            tab1CreatePublic.AddChild(new TextField(40, 24 + 40 + 60, Globals.WIDTH/3f-80, 40, ""));
            tab1CreatePublic.AddChild(new Label(40, 84 + 40 + 60, Globals.WIDTH/3f-80, 24, "Room Description", textFieldLabelStyle));
            tab1CreatePublic.AddChild(new TextField(40, 108 + 40 + 60, Globals.WIDTH/3f-80, 40, ""));
            tab1CreatePublic.AddChild(new Checkbox(40, 168 + 40 + 60, Globals.WIDTH/3f-80, 40, "Mark room as NSFW (not safe for work)"));
            tab1CreatePublic.AddChild(new Button(40, 168+40+60+40+20, Globals.WIDTH/3f - 80, 40, "Create", onClick: () => {
                
            }));
            tab1CreatePublic.AddChild(new Button(40, Globals.HEIGHT-80, Globals.WIDTH/3f-80, 40, "Back", onClick: () => {
                tab1CreatePublic.x = -100000f;
                tab1Main.x = 0f;
            }));
            tab1CreatePrivate.AddChild(new Label(40, 0 + 40, Globals.WIDTH/3f - 80f, 40, "Create private room", primaryTitleStyle));
            tab1CreatePrivate.AddChild(new Label(40, 0 + 40 + 60, Globals.WIDTH/3f - 80, 24, "Room Name", textFieldLabelStyle));
            tab1CreatePrivate.AddChild(new TextField(40, 24 + 40 + 60, Globals.WIDTH/3f - 80, 40, ""));
            tab1CreatePrivate.AddChild(new Label(40, 84 + 40 + 60, Globals.WIDTH/3f - 80, 24, "Room Description", textFieldLabelStyle));
            tab1CreatePrivate.AddChild(new TextField(40, 108 + 40 + 60, Globals.WIDTH/3f - 80, 40, ""));
            tab1CreatePrivate.AddChild(new Label(40, 168 + 40 + 60, Globals.WIDTH/3f - 80, 24, "Room Code", textFieldLabelStyle));
            tab1CreatePrivate.AddChild(new TextField(40, 192 + 40 + 60, Globals.WIDTH/3f - 80, 40, ""));
            tab1CreatePrivate.AddChild(new Checkbox(40, 252 + 40 + 60, Globals.WIDTH/3f - 80, 40, "Mark room as NSFW (not safe for work)"));
            tab1CreatePrivate.AddChild(new Button(40, 252 + 40 + 60+40+20, Globals.WIDTH/3f - 80, 40, "Create", onClick: () => {
                
            }));
            tab1CreatePrivate.AddChild(new Button(40, Globals.HEIGHT-80, Globals.WIDTH/3f - 80, 40, "Back", onClick: () => {
                tab1CreatePrivate.x = -100000f;
                tab1Main.x = 0f;
            }));
            
            tab2.AddChild(new Label(0, 50, Globals.WIDTH/3f, 50, "JOIN PRIVATE ROOM", primaryTitleStyle));
            tab2.AddChild(new Label(0, 130, Globals.WIDTH/3f, 50, "Enter room code:", secondaryTitleStyle));
            tab2.AddChild(new TextField(0, 180, Globals.WIDTH/3f - 60, 50, "code", TextFieldStyle.Default));
            tab3.AddChild(new Label(0, 50, Globals.WIDTH/3f, 50, "JOIN PUBLIC ROOM", primaryTitleStyle));
            tab3.AddChild(new Button(40, Globals.HEIGHT-80f, Globals.WIDTH/3f-80, 40, "Refresh", onClick: () => {
                
            }));
            var roomButtonStyle = ButtonStyle.Default.Alter(backgroundColorNormal:Color.FromArgb(0,255,255,255), backgroundColorHover:Color.FromArgb(0,255,255,255),backgroundColorPressed:Color.FromArgb(0,255,255,255),borderSizeHover:0,borderSizeNormal:0,borderSizePressed:0);
            var room1 = UIFactory.CreateJoinPublicRoomEntry("Room 1", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse facilisis commodo dui, vel facilisis mauris commodo porttitor. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec ut fringilla turpis.", true, "123");
            var room2 = UIFactory.CreateJoinPublicRoomEntry("Room 2", "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse facilisis commodo dui, vel facilisis mauris commodo porttitor. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec ut fringilla turpis.", false, "11111aaaaaa");
            room1.y = 100f;
            room2.y = 320f;
            tab3.AddChild(room1);
            tab3.AddChild(room2);
            IsLoaded = true;
        }
    }
}