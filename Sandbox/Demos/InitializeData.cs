
using System;
using FluidDbClient.Shell;
using FluidDbClient.Sql;

namespace FluidDbClient.Sandbox.Demos
{
    public static class InitializeData
    {
        private const string Script =
@"
DELETE FROM Robot;
DELETE FROM Widget;

INSERT INTO Robot (Name, Description, DateBuilt, IsEvil) SELECT * FROM @Robots;
INSERT INTO Widget (GlobalId, Name, Description) SELECT * FROM @Widgets;
";

        public static void Start()
        {
            var robots = CreateRobotData();
            var widgets = CreateWidgetData();
            
            Db.Execute(Script, new { robots, widgets });
        }

        private static StructuredData CreateRobotData()
        {
            var data =
                new NewRobotsDataBuilder()
                .AppendValues("Sonny", "Featured in I, Robot", new DateTime(1999, 5, 28), false)
                .AppendValues("Rosie", "Cleaning bot in The Jetsons", new DateTime(1965, 11, 17), false)
                .AppendValues("R2-D2", "Bleep bleep bloop OWEEE!", new DateTime(1976, 2, 14), false)
                .AppendValues("WALL-E", "Trash bundler and cosmic traveller", new DateTime(2010, 6, 2), false)
                .AppendValues("Bender", "Featured in Futurama", new DateTime(2004, 9, 15), false)
                .AppendValues("Optimus Prime", "Supreme commander of the Transformers ", new DateTime(1986, 3, 11), false)
                .AppendValues("C-3PO", "Protocol droid; friends with R2-D2", new DateTime(1976, 12, 29), false)
                .AppendValues("Terminator", "I'll be back", new DateTime(1986, 1, 20), true)
                .AppendValues("Data", "Featured in Star Trek: The Next Generation", new DateTime(1997, 8, 5), false)
                .AppendValues("The Iron Giant", "Hulking but friendly", new DateTime(2004, 4, 8), false)
                .AppendValues("HAL 9000", "Open the pod door, Dave", new DateTime(2001, 7, 16), true)
                .AppendValues("RoboCop", "Wears a badge and goes on patrol", new DateTime(1989, 2, 24), false)
                .AppendValues("Eve", "Ecologist; girlfriend of WALL-E", new DateTime(2010, 5, 7), false)
                .AppendValues("Johnny5", "Featured in Short Circuit", new DateTime(1983, 2, 14), false)
                .AppendValues("Dalek", "Featured in Dr. Who", new DateTime(1979, 10, 21), false)
                .AppendValues("Number Six", "Featured in Battlestar Galactica", new DateTime(2006, 10, 29), false)
                .AppendValues("Lost in Space Robot", "The robot...from Lost in Space(!)", new DateTime(1954, 6, 2), false)
                .AppendValues("KITT", "The badass technology behind Knight Rider", new DateTime(1982, 11, 3), false)
                .AppendValues("Bishop", "In Alien, you're like...what?...he's a robot!", new DateTime(1980, 3, 5), false)
                .AppendValues("Cylon", "Featured in Battlestar Galactica", new DateTime(2002, 5, 7), true)
                .AppendValues("Bumblebee", "Another Transformer", new DateTime(1990, 4, 11), false)
                .AppendValues("ED-209", "Featured in RoboCop", new DateTime(1986, 12, 13), true)
                .AppendValues("Tom Servo", "In Mystery Science Theater 3000; made from gumball machine", new DateTime(1992, 1, 17), false)
                .AppendValues("Ultron", "Marvel super villain", new DateTime(1971, 8, 23), true)
                .AppendValues("Gort", "Iconic robot from The Day the Earth Stood Still", new DateTime(1958, 9, 29), true)
                .AppendValues("Robby the Robot", "Featured in Forbidden Planet", new DateTime(1952, 7, 18), false)
                .AppendValues("GIR", "Featured in Invader Zim", new DateTime(1986, 11, 12), false)
                .AppendValues("Mechagodzilla", "Mech + Godzilla = Mechagodzilla, duh", new DateTime(1982, 4, 30), true)
                .AppendValues("Crow T. Robot", "In Mystery Science Theater 3000", new DateTime(1992, 8, 28), false)
                .AppendValues("Calculon", "Featured in Futurama", new DateTime(2002, 10, 26), false)
                .AppendValues("K-9", "Dr. Who robot dog", new DateTime(1988, 6, 24), false)
                .AppendValues("General Grievous", "4 light sabre swinger", new DateTime(2003, 1, 7), true)
                .AppendValues("Mega Man", "The name says it all", new DateTime(1985, 9, 20), false)
                .AppendValues("Voltron", "Everything a robot should be in the 80's", new DateTime(1983, 10, 18), false)
                .AppendValues("The Mars Rover", "The red planet's sample collector and photographer", new DateTime(2000, 4, 16), false)
                .AppendValues("BB-8", "Roly poly adventurer", new DateTime(2015, 2, 23), false)
                .AppendValues("V.I.N.CENT", "Featured in the Black Hole", new DateTime(1981, 7, 12), true)
                .AppendValues("GLaDOS", "Featured in Half-Life and Portal", new DateTime(2012, 12, 10), false)
                .AppendValues("Cyberman", "The best Dr. Who villian of all time", new DateTime(1979, 6, 8), true)
                .AppendValues("Baymax", "Marshmallow-like robot", new DateTime(2013, 5, 19), false)
                .AppendValues("Maximillion", "Featured in The Black Hole", new DateTime(1986, 2, 25), false)
                .AppendValues("Maria Robot", "The iconic robot in Metropolis", new DateTime(1921, 3, 31), false)
                .AppendValues("Inspector Gadget", "Always full of surprises", new DateTime(1995, 5, 31), false)
                .AppendValues("Android 18", "Featured in Dragon Ball-Z", new DateTime(1996, 7, 31), false)
                .AppendValues("Kryten", "Featured in Red Dwarf", new DateTime(2012, 9, 28), true)
                .AppendValues("Ratchet", "Featured in The Transformers", new DateTime(1996, 11, 24), false)
                .AppendValues("Wheatley", "Featured in Portal 2", new DateTime(1986, 10, 31), false)
                .AppendValues("Clank", "A popular videogame sidekick", new DateTime(2011, 8, 30), false)
                .AppendValues("Brainiac", "Foe to Superman", new DateTime(1983, 6, 28), true)
                .AppendValues("Dot Matrix", "Featured in Spaceballs", new DateTime(1985, 4, 27), false)
                .AppendValues("Goddard", "Featured in The Adventures of Jimmy Neutron", new DateTime(1999, 2, 26), false)
                .AppendValues("Gypsy", "In Mystery Science Theater 3000; assembled from vacuum cleaner parts", new DateTime(1992, 1, 25), false)
                .AppendValues("Geoffrey Peterson", "As seen on the Late Late Show", new DateTime(2014, 3, 24), false)
                .AppendValues("Roomba", "Cleans your house", new DateTime(2008, 5, 23), false)
                .AppendValues("Red Tornado", "From the DC Universe", new DateTime(1976, 7, 22), true)
                .AppendValues("Megazord", "Named this due to his big...zord", new DateTime(1963, 9, 21), false)
                .AppendValues("Domesticon", "Built to serve...martinis", new DateTime(2005, 11, 20), false)
                .AppendValues("Tachikoma", "From Ghost in the Shell", new DateTime(1993, 12, 19), false)
                .Build();

            return data;
        }

        private static StructuredData CreateWidgetData()
        {
            var data =
                new NewWidgetsDataBuilder()
                .AppendValues(Guid.NewGuid(), "Whirligig", "Spins and spins and spins...")
                .AppendValues(Guid.NewGuid(), "The Re-combobulator", "Puts it all back together for you!")
                .AppendValues(Guid.NewGuid(), "Spiralizer", "Makes spirals out of just about anything")
                .AppendValues(Guid.NewGuid(), "Non-Linear VectorPhone", "Used to call into random vortexes")
                .AppendValues(Guid.NewGuid(), "Logic Monkey", "Eats your math homework while making cute noises")
                .AppendValues(Guid.NewGuid(), "Jar Jar Remover", "Filters out all references to Jar Jar in media")
                .AppendValues(Guid.NewGuid(), "Jello Wizard", "Just add water and watch it work")
                .Build();

            return data;
        }
    }
}
