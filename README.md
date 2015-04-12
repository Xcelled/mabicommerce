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

You can download the lastest release from the [Releases tab](/../../releases).

Once it's downloaded, simply extract it to a folder on your desktop, or somewhere where you can find it.

**If you wish to enable AutoDetect, you need to do the following:**

1. Download (and extract) Morrighan from [the Aura project](http://aura-project.org/forum/index.php/topic/1082-morrighan-client-proxy-updated-2015-03-01-v121b/#entry8050).
	***Note: Morrighan is still in a "beta" stage and does have some bugs.*** Check the Morrighan thread frequently for updates, especially if you randonly "freeze" or become unable to move!
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
4. Select any modifiers you might have.
5. Talk to a goblin and trade!
6. If you're using AutoDetect, MabiCommerce should have filled all the information in for you. If not, you'll have to enter the prices and profits for each item by hand, as well as the stock, your current ducats, and merchant level.
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
 - Loading times of maps (It knows it's faster to leave Tara than enter it!)
 - If the route has anti-bot measures applied (Like Cobh-Dunby)
 - If the route passees through a bandit "choke point" like Osna
 - Mixed loads

And here is what MabiCommerce tells you about each trade:

 - Score: This is how "good" the trade is. It represents how many ducats you earn for every second of trading.
 - Total Profit: The total amount you'll earn, in ducats. This is also the amount of gold and merchant rating you'll get.
 - Total Cost: How much you'll pay to buy the load.
 - Flags: Extra route information, such as No Profit or Choke Point.