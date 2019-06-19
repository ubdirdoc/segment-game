using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity;
using UnityEngine;

namespace SEGMent.Json
{
    [System.Serializable]
    public class Metadata
    {
        public string Label;
    }

    [System.Serializable]
    public class Sound
    {
        public string Path;
        public float Range;
        public float Volume;
        public bool Repeat;
    }

    [System.Serializable]
    public class Object
    {
        public string Path;
        public string Image;
        public float[] Pos;
        public float[] Size;
        public Sound Sound;
        public bool PuzzlePiece;
        public int Z;

        [NonSerialized]
        internal int id = -1;
        [NonSerialized]
        internal Scene scene = null;
    }

    [System.Serializable]
    public class Gif
    {
        public string Path;
        public string Image;
        public float[] Pos;
        public float[] Size;
        public Sound Sound;
        public int Default;
        public int[] Frames;
        public int Z;

        [NonSerialized]
        internal int id = -1;
        [NonSerialized]
        internal Scene scene = null;
    }

    [System.Serializable]
    public class ClickArea
    {
        public string Path;
        public float[] Pos;
        public float[] Size;
        public Sound Sound;
        public int Z;

        [NonSerialized]
        internal int id = -1;
        [NonSerialized]
        internal Scene scene = null;
    }
    [System.Serializable]
    public class BackClickArea
    {
        public string Path;
        public float[] Pos;
        public float[] Size;
        public Sound Sound;
        public int Z;

        [NonSerialized]
        internal int id = -1;
        [NonSerialized]
        internal Scene scene = null;
    }
    [System.Serializable]
    public class TextArea
    {
        public string Path;
        public float[] Pos;
        public float[] Size;
        public Sound Sound;
        public string Text;
        public int Behaviour;
        public int Z;

        [NonSerialized]
        internal int id = -1;
        [NonSerialized]
        internal Scene scene = null;
    }

    [System.Serializable]
    public class Scene
    {
        public string Path;
        public Metadata Metadata;
        public float[] Rect;
        public float[] ImageSize;
        public int SceneType;
        public string StartText;
        public bool RepeatText;
        public string Image;
        public Sound Ambience;
        public Object[] Objects;
        public Gif[] Gifs;
        public ClickArea[] ClickAreas;
        public BackClickArea[] BackClickAreas;
        public TextArea[] TextAreas;

        [NonSerialized]
        internal int id = -1;
    }

    [System.Serializable]
    public class GifRiddle { }
    [System.Serializable]
    public class PuzzleRiddle { }
    [System.Serializable]
    public class TextRiddle
    {
        public string Question;
        public string Expected;
        public string IfCorrect;
        public string IfWrong;
        public string[] FuzzyMatches;
        public bool UseStars;
    }


    [System.Serializable]
    public class Riddle
    {
        public GifRiddle Gif;
        public TextRiddle Text;
        public PuzzleRiddle Puzzle;
        public string Which;
    }

    [System.Serializable]
    public class SceneToScene
    {
        public string From;
        public string To;
        public Riddle Riddle;
    }

    [System.Serializable]
    public class ObjectToScene
    {
        public string From;
        public string To;
    }

    [System.Serializable]
    public class ClickAreaToScene
    {
        public string From;
        public string To;
    }

    [System.Serializable]
    public class TransitionImpl
    {
        public SceneToScene SceneToScene;
        public ObjectToScene ObjectToScene;
        public ClickAreaToScene ClickAreaToScene;
        public string Which;
    }

    [System.Serializable]
    public class TransitionBase
    {
        public int Fade;
        public float[] Color;
        public Sound Sound;
        public TransitionImpl Transition;
    }


    [System.Serializable]
    public class Game
    {
        public Scene[] Scenes;
        public TransitionBase[] Transitions;
    }

    [System.Serializable]
    public class Document
    {
        public Game Process;
    }
    [System.Serializable]
    public class Root
    {
        public Document Document;
    }

    public class LoadJson
    {
        public LoadJson()
        {
        }

        private string SanitizeSound(string s)
        {
            if(s.StartsWith("Sounds/"))
            {
                s = s.Remove(0, 7);
            }
            if(s.EndsWith(".wav"))
            {
                s = s.Remove(s.Length - 4);
            }
            return s;
        }

        BoundingBox itemBox(float[] pos, float[] size, Scene scene)
        {
        	return BoundingBox.FromRect(
                pos, 
                new float[] { size[0], size[1] * (scene.ImageSize[0] / scene.ImageSize[1]) }
            );

        }

        public void Load(string json, Player player)
        {
            int start = 80;
            int end = 68;
            string actual = json.Substring(start, json.Length - start - end);
            Game game = JsonUtility.FromJson<Game>(actual);

            var rooms = new GameStructureRooms(player.GetInformationManager());

            var pathToScene = new Dictionary<string, Scene>();
            var pathToItem = new Dictionary<string, Object>();
            var pathToGif = new Dictionary<string, Gif>();
            var pathToClick = new Dictionary<string, ClickArea>();
            var pathToBackClick = new Dictionary<string, BackClickArea>();
            var pathToText = new Dictionary<string, TextArea>();

            Scene initialRoom = null;
            foreach (var scene in game.Scenes)
            {
                scene.id = rooms.CreateRoom();
                pathToScene[scene.Path] = scene;

                rooms.SetRoomBackgroundImageURL(scene.id, scene.Image);
                rooms.SetRoomBackgroundMusic(scene.id, SanitizeSound(scene.Ambience.Path), scene.Ambience.Repeat);
                rooms.SetRoomDescription(scene.id, scene.StartText, scene.RepeatText);

                // Types: 0 = default, 1 = initial, 2 = final, 3 = game over
                if (scene.SceneType == 1) 
                    initialRoom = scene;
                // TODO rooms.SetRoomDiaryEntry(id, rooms.Value.diaryItem, rooms.Value.newDiaryItemMustBeHighlighted);

                // Sort all the items by Z
                Array.Sort<Object>(scene.Objects, (x,y) => x.Z.CompareTo(y.Z));
                Array.Sort<Gif>(scene.Gifs, (x,y) => x.Z.CompareTo(y.Z));

                foreach (var item in scene.Objects)
                {	
                    item.id = rooms.CreateItem(scene.id, itemBox(item.Pos, item.Size, scene));
                    item.scene = scene;
                    if (item.id != GameStructureRooms.CREATION_ERROR)
                    {
                        pathToItem[item.Path] = item;

                        Debug.Log(item.Image);
                        rooms.SetItemBackgroundImageURL(item.id, item.Image);
                        if (item.Sound.Path.Length > 0)
                        { 
                            rooms.SetItemSoundName(item.id, SanitizeSound(item.Sound.Path));
                        }
                        // if (item.Description.length > 0)
                        // todo rooms.SetItemDescription(item.id, "");
                        if (item.PuzzlePiece)
                        {
                            rooms.SetItemIsPuzzle(item.id, true);
                        }
                    }
                }

                foreach (var item in scene.Gifs)
                {
                    item.id = rooms.CreateItem(scene.id, itemBox(item.Pos, item.Size, scene));
                    item.scene = scene;
                    if (item.id != GameStructureRooms.CREATION_ERROR)
                    {
                        pathToGif[item.Path] = item;
                        rooms.SetItemBackgroundImageURL(item.id, item.Image);
                        if (item.Sound.Path.Length > 0)
                            rooms.SetItemSoundName(item.id, SanitizeSound(item.Sound.Path));
                        // if (item.Description.length > 0)
                        // todo rooms.SetItemDescription(item.id, "");
                        rooms.SetItemStartFrame(item.id, item.Default);

                        for (int i = 0; i < item.Frames.Length; i++)
                        {
                            switch(item.Frames[i])
                            {
                                case 0: // Ignored frame
                                    break;
                                case 1:
                                    rooms.AddItemStopState(item.id, i + 1);
                                    break;
                                case 2:
                                    rooms.AddItemStopState(item.id, i + 1);
                                    rooms.AddItemSolutionState(item.id, i + 1);
                                    break;
                            }
                        }
                    }
                }

                foreach (var item in scene.ClickAreas)
                {
                    item.scene = scene;
                    pathToClick[item.Path] = item;
                }

                foreach (var item in scene.BackClickAreas)
                {
                    item.scene = scene;
                    pathToBackClick[item.Path] = item;
                    int trans_id = rooms.CreateBackTransition(
                        scene.id
                        , itemBox(item.Pos, item.Size, scene)
                        , true);
                    if(item.Sound.Path.Length > 0)
                        rooms.SetTransitionSound(trans_id, SanitizeSound(item.Sound.Path));
                }

                foreach (var item in scene.TextAreas)
                {
                    item.id = rooms.CreateClickText(scene.id, itemBox(item.Pos, item.Size, scene));
                    item.scene = scene;
                    if (item.id != GameStructureRooms.CREATION_ERROR)
                    {
                        pathToText[item.Path] = item;
                        if (item.Sound.Path.Length > 0)
                            rooms.SetClickTextSoundName(item.id, SanitizeSound(item.Sound.Path));

                        string txt = item.Text;
                        switch(item.Behaviour)
                        {
                            case 0:
                                txt = txt.Insert(0, "+");
                                break;
                            case 1:
                                break;
                            case 2: // CLEAR
                                break;
                            case 3:
                                txt = "!!!";
                                break;
                        }
                        rooms.SetClickTextText(item.id, txt, /* currentClickTextToAdd.isURL */ false);
                    }
                }
            }

            foreach(var trans in game.Transitions)
            {
                int? trans_id = null;
                bool is_immediate = trans.Fade == 0;
                switch(trans.Transition.Which)
                {
                    case "SceneToScene":
                    {
                        var t = trans.Transition.SceneToScene;
                        var source = pathToScene[t.From];
                        var target = pathToScene[t.To];
                        
                        switch(t.Riddle.Which)
                        {
                            case "Gif":
                            {
						        trans_id = rooms.CreateStateObjectSolutionTransition(
                                    source.id
                                    , target.id
                                    , is_immediate, false);
                                break;
                            }
                            case "Text":
                            {
                                var solutions = new List<string>{t.Riddle.Text.Expected};
                                if (t.Riddle.Text.FuzzyMatches != null)
                                {
                                    for(int i = 0; i < t.Riddle.Text.FuzzyMatches.Length; i++)
                                    {
                                        solutions.Add(t.Riddle.Text.FuzzyMatches[i]);
                                    }
                                }

                                var wrong_answers = new Dictionary<string, string>();

                                if(t.Riddle.Text != null && t.Riddle.Text.IfWrong.Length > 0)
                                    wrong_answers["__default__"] = t.Riddle.Text.IfWrong;

                                trans_id = rooms.CreateSolutionTransition(
                                    source.id
                                    , target.id
                                    , t.Riddle.Text.Question
                                    , solutions
                                    , wrong_answers
                                    , t.Riddle.Text.UseStars, is_immediate, false);
                                break;
                            }
                            case "Puzzle":
                            {
						        trans_id = rooms.CreatePuzzleSolutionTransition(
                                    source.id
                                    , target.id
                                    , is_immediate, false);

                                break;
                            }
                            default:
                            {
                                break;
                            }
                        }
                        break;
                    }
                    case "ObjectToScene":
                    {
                        var t = trans.Transition.ObjectToScene;
                        var source = pathToItem[t.From];
                        var target = pathToScene[t.To];

                        trans_id = rooms.CreateClickableTransition(
                            source.scene.id, target.id
                            , itemBox(source.Pos, source.Size, source.scene)
                            , is_immediate, false);
                        break;
                    }
                    case "ClickAreaToScene":
                    {
                        var t = trans.Transition.ClickAreaToScene;
                        var source = pathToClick[t.From];
                        var target = pathToScene[t.To];

                        trans_id = rooms.CreateClickableTransition(
                            source.scene.id, target.id
                            , itemBox(source.Pos, source.Size, source.scene) 
                            , is_immediate, false);
                        break;
                    }
                    case "GifToScene":
                    {
                        // does this happen ? it would be weird since gifs are used for puzzles
                        break;
                    }
                    default:
                    {
                        break;
                    }
                }

                if(trans_id != null && trans.Sound.Path.Length > 0)
                {
                    rooms.SetTransitionSound(trans_id.Value, SanitizeSound(trans.Sound.Path));
                }
            }
            
            if(initialRoom != null)
            {
                rooms.TeleportToRoom(initialRoom.id);
                player.SetMap(rooms);
            }
        }
    }
}
