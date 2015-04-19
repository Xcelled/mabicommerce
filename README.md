# MabiCommerce
Thank you for using MabiCommerce. Please make sure to also read the [FAQ](/../FAQ.md).

## Quick Start
### What is MabiCommerce?

MabiCommerce is a tool to help you get the most out of your trades! MabiCommerce will take a variety of factors, toss them into a magical calculating machine, grind it around, and show you a list of all the trades you can make, and their associated stats. Optionally, MabiCommerce can automatically detect and fill in the information it needs.

![MabiCommerce Demo](/../screenshots/demo1.gif?raw=true)

### Basic Features
 - Takes a large number of factors into account to get YOU the best trade.
 - Warns you about "no profit" routes or routes that have unavoidable bandits.
 - Shows you the best route to take.
 - Supports auto detection of values, so you don't need to type.

### Where do I get it?

You can download the latest release from the [Releases tab](/../../releases).

Once it's downloaded, simply extract it to a folder on your desktop, or somewhere where you can find it.

**If you wish to enable AutoDetect, you need to do the following:**

1. Download (and extract) Morrighan from [the Aura project](http://aura-project.org/forum/index.php/topic/1082-morrighan-client-proxy-updated-2015-03-01-v121b/#entry8050).
	***Note: Morrighan is still in a "beta" stage and does have some bugs.*** Check the Morrighan thread frequently for updates, especially if you randomly "freeze" or become unable to move!
2. Copy `Morrighan.exe` (from Aura's website) and `MorriOneClick.exe` (from MabiCommerce's folder) to your client directory\*.
3. Shut down Mabinogi if it is running.
4. Run MorriOneClick to start the game. You may create a shortcut to MorriOneClick on your desktop to make it easier.
5. If you ever get a "HackShield" error, you simply need to patch your game by running Mabinogi normally (NOT through MorriOneClick!) and then re-opening it with MorriOneClick.

\*Your client directory is located at different places depending on how you installed the game:

- The most common place is `C:\Nexon\Mabinogi`
- If you used Steam, it'll be in your [steamapps](https://support.steampowered.com/kb_article.php?ref=7418-YUBN-8129) folder.
- If you used the new Nexon launcher, it'll be `C:\Nexon\Library\mabinogi\appdata` 

If you see a file called `client`, you're in the right place. Copy Morrighan and MorriOneClick here!

### How do I use it?

***It is highly recommended to put Mabinogi into Windowed mode when using MabiCommerce.***

1. If you're using AutoDetect, make sure you started the game with MorriOneClick. You should see a black box with an `M` in it in the upper left corner of your screen.
2. Run MabiCommerce by double clicking on it.
3. If you're using AutoDetect, click connect. If not, enter your information like your transports.
4. Select any modifiers you might have. Be warned, large numbers of modifiers may slow down the application! See the `Modifiers` section later in this document for details.
5. Talk to a goblin and trade!
6. If you're using AutoDetect, MabiCommerce should have filled all the information in for you. If not, you'll have to enter the prices and profits (red/blue numbers!) for each item by hand, as well as the stock, your current ducats, and merchant level.
7. Press "Calculate Trades". The button will change appearance and become unclickable until calculation is finished. This may take several seconds, depending on your computer.
8. Results are displayed below the Calculate button. By default, they're sorted by the item score, but you can click the column headers to change their sort order.
9. Click on a result to view the type and quantity of items in the load and transportation method.
10. Optionally click "Map It" to have MabiCommerce show you the fastest route. Clicking a green region will bring up a minimap with the best route shown in blue.

***Note:*** If you talk to a goblin and then restart MabiCommerce, MabiCommerce will **not** detect profit information *for that trading post only* for approximately five minutes. This is due to client caching of prices. If it's a big deal for you, you can change channels to immediately restore AutoDetect.

## How does MabiCommerce work?

Here's a laundry list of what MabiCommerce takes into account when calculating trades:

 - Your current Merchant Level
 - Your current Ducats
 - The current cost of the item
 - The profit of each item at each town
 - The current stock of the item
 - Your available transportation, and their associated weights, speeds, and slots
 - Your modifiers, such as a Commerce Partner or Alpaca
 - Distance/Time between trading posts
 - Loading times of maps
 - Flags (bandit choke point, no profit)
 - Mixed loads

And here is what MabiCommerce tells you about each trade:

 - Score: This is how "good" the trade is. It represents how many ducats you earn for every second of trading.
 - Profit: The total amount you'll earn, in ducats.
 - Gold: The total amount of gold you'll earn.
 - Merchant Rating: The total amount of merchant rating you'll receive.
 - Cost: How much you'll pay to buy the load.
 - Flags: Extra route information, such as No Profit or Choke Point.
 - Exp: The amount of experience you'll get for the load.

#### Merchant Rating

MabiCommerce uses your Merchant Rating to determine the level of goods you can buy, as well as the discount you get (AutoDetect only).

#### Ducats

MabiCommerce uses your Ducat amount to determine how much you can buy. Since it requires around 400,000 Ducats to be able to buy a full load of the most expensive item, this metric is *very* important.

#### Current Cost

Used in conjunction with Ducats to calculate how much of an item you can buy.

#### Profit of each item

Obviously an important measurement, the profit (a red or blue number) reflects how much you earn per unit at that particular post.

#### Item Stock

Very important for limited time items, the stock is another factor that limits how much you can buy.

#### Transportation

Each mode of transportation has a different weight capacity, slot capacity, and speed bonus. While the elephant and wagon beat out the handcart and the backpack, which of those two is better?

Many traders think that the wagon is better, because of its speed bonus and thus, they don't bother buying the elephant. This is a big mistake as the elephant's bigger weight capacity makes it a must-have for some towns like Bangor.

The question remains: Horse's slots and speed or Elephant's weight? The answer is... it depends on what you're taking where. Usually, it's a pain to figure this out. MabiCommerce will calculate the results for each, eliminating guesswork.

#### Modifiers (Commerce Partner, Alpaca, ...)

If you have a Commerce Partner or Alpaca, one of more of your transportation methods receives an upgrade to weight, slots, or both, essentially turning it into a new form of transportation. Not taking this into account results in some hefty missed profits.

Additionally, since MabiCommerce 2.0.2, Alpaca Robes and Letters of Guarantee are supported to improve accuracy.

MabiCommerce will evaluate all combinations of enabled modifiers (the [power set](http://en.wikipedia.org/wiki/Power_set), for you math geeks) to find the best combination for you.

**Note:** Because the size of the *n*th power set is `2^n`, enabling a large number of modifiers can drastically lengthen the time it takes to calculate trades. For example, while 2 modifiers only increases complexity by a factor of 4, enabling 9 modifiers will cause calculation to take **512** times as long. This is the difference between MabiCommerce doing its work in 4 seconds vs 8 minutes!

For those of you wondering: Yes, I have optimized the power set algorithm. It will [short circuit](http://en.wikipedia.org/wiki/Short-circuit_evaluation) "illegal" states, such as multiple letters. This should severely reduce the amount of sets actually generated, but the above is still worth bearing in mind.

#### Total Time

> Always check the market price and head into the direction of highest profit, **but keep the travel times in mind**.
>
> -- <cite>Commerce Goblin</cite>

MabiCommerce, unlike nearly every other method of calculating commerce information, heeds the Goblin's advice. I spent an afternoon running around Erinn, marking down the coordinates of various waypoints. MabiCommerce loads these and connects them via a structure called a **directed graph**, to make a spiderweb of routes across Erinn. By using [Djikstra's Algorithm](http://en.wikipedia.org/wiki/Dijkstra's_algorithm), MabiCommerce knows the shortest way to get from point A to point B.

Once it knows how far apart the posts are, it uses the game's human running speed (3.43 meters/second) to calculate the total time required to get there. Total profit of the route is divided by total time to run it, yielding `profit/second`, a measure of how efficient a given trade is. High efficiency trades earn you more Ducats faster, even though individually, they may give less profit. See also [the FAQ](/../FAQ.md).

Here is how MabiCommerce sees Dunbarton, with potential paths highlighted in blue:

![Trade Routes](/../screenshots/dunby_waypoints.png?raw=true)

#### Loading times of maps

Any experienced Mabinogi player knows what Tara lag is. It's the minute or so you can't use your computer as the game loads you into Tara. This represents 60 seconds of lost trading time, enough to drastically alter profit margins. I measured the time it took to load each map, and MabiCommerce uses this information in its calculation of the time required to reach a destination trading post.

MabiCommerce takes this one step further, however. It knows that leaving Tara is faster than entering, and so it intelligently makes use of one of *two* times, depending on if the route is entering or leaving the map.

#### Flags

MabiCommerce has the ability to "flag" trades if they meet certain criteria. Right now, two flags exist: `ChokePoint` and `NoProfit`.

MabiCommerce will report flags in the `Flags` column and may also color-code the trade's entry:

![Flags](/../screenshots/flags.png?raw=true)

##### Choke Point

MabiCommerce flags as trade as a **Choke Point** if the route passes through a tight map, where bandits may be impossible to avoid if they appear. Traders who have trouble with bandits may want to avoid these routes, or seek assistance before embarking on them.

The current list of choke point maps is:

 - Osna Sail
 - Corrib Valley
 - Dugald Isle

##### No Profit

Nexon, in an attempt to curb early botters, removed profit from certain routes. MabiCommerce flags these as **no profit** routes. If you embark on a no-profit trade, you will, at most, earn enough to cover what you paid for your items. This results in a **zero or negative** profit for the trade, and thus no earned gold, exp, or merchant rating. Avoid these routes.

The current list of no-profit routes is:

 - Tir Chonaill <-> Taillteann
 - Cobh <-> Dunbarton
 - Cobh <-> Belvast
 - Taillteann <-> Tara

#### Mixed Loads

> MabiCommerce instructed me to include 1 seaweed in a load from Cobh because it filled up the very last bit of space, in order to absolutely maximize my profit. I LOVE IT.
>
> -- <cite>Rydian</cite>

A common occurrence while trading is that, after you buy the maximum number of some item, you still have slots, weight, and ducats left to buy other items. Instead of wasting these slots, MabiCommerce will fill them with other items, so you truly get the most bang for your, err... Ducat. MabiCommerce will even explore the possibility of rounding down to the nearest whole-slot, instead of leaving one slot partially filled.

For more details, see the [Algorithm](#Algorithm) section.

Here is an example of MabiCommerce advising a mixed load of `Highlander Ore` and `Topaz`, to get the most profit.

![Mixed Loads](/../screenshots/mixed_loads.png?raw=true)

### Algorithm

Ever wanted to know how MabiCommerce does what it does, but you don't want to go spelunking through the source code? Then this section is for you. Following is the magic at MabiCommerce's heart, beautifully distilled and rendered as pseudo code:

````
CODE GOES HERE.
````

## Acknowledgments

MabiCommerce would not have happened without the contributions of these wonderful people:

 - Huge, huge thank you to **Rydian** (http://rydian.net/) for his countless hours of testing, screenshotting, debugging, feedback, writing, and suggestions.
 - Thank you to **Exec** (http://mabinoger.com/) for developing Morrighan and helping debug some issues with AutoDetect.
 - Thank you to **Plonecakes** (https://github.com/Plonecakes/) for being my rock during development, double checking my math, and figuring out that my merchant rating was the reason MabiCommerce was detecting prices 2% higher than they were.
 - Thank you to the men and women of the **Mabinogi World Wiki** (http://wiki.mabinogiworld.com/) for their tireless research on the game, including the `Exp` formula for commerce.
 - Thank you to **Hanae** (http://xerodox.com/) for helping me with the original MabiCommerce's web site and giving me design tips.
 - Thank you to **Cosmos** (http://cosrnos.com/) for his help with UI design, something I'm notoriously bad at!
 - Thank you to **devCAT** and **Nexon** (http://mabinogi.nexon.net/) for making an awesome (yet horribly coded at times) game, and sharing it with the world.

And finally, thank you, loyal users of MabiCommerce, who helped the original be downloaded over ***2,000*** times!