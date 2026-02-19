CHANGES BY ME
Changes in the SimpleMiner.cs file:
Added random number generator on line 18
Added a new state called "focusedMiningState" on line 24 and line 34
Line 56 - 76, added state transitions for the focused mining state
Line 158 - 172, added a private void titled "focusedDig" which allows the player to enter a "focused" mode if the miner's thirst is less than 8. While in this state, the miner has a 25% chance of getting a critical hit, which gives +3 gold
Lines 186 and 188 include 2 booleans that check whether or not the miner's thirst will allow them to be in this focused state

Changes in Program.cs file:
Line 12, changed the gameLengthInTicks integer to equal 60 ticks
