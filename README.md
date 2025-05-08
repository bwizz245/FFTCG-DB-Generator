## How To Use It
To use this program, run the DBGen.exe file from the command line. The program has 2 parameters, both of which are paths. The first parameter refers to the card data file that the program will read from, and the second refers to the intended output database file, which must be a .db file. The current release only has binaries for 64-bit Windows, so if you're using another OS you'll have to build it yourself.

#### Example
The following example assumes that the .exe file is located in a directory that contains both an image subdirectory and the file cards.json, which contains the card data.

`DBGen.exe './cards.json' './carddata.db'`

This command will open `carddata.db` (or create it if it doesn't already exist), read the data from `cards.json`, then input that data into `carddata.db`.

The program will catch errors in the input data and print them to the console, so if you mess something up you'll probably be able to see it.

## The Card Data
The input card data is in the form of a json file listing each card and its relevant information, along with individual image files for each of the cards (they can be in the same directory as the card file itself, but I would advise putting them in their own subdirectory). The json file must contain a single object with each card as an individual property.

The following are templates for card entries, one for Vivosaur cards and the other for Action cards. Note that the simulator will not use FP or Status cards, so they are not necessary for this.

```json
"[Card Number]": {
    "name": "",
    "type": "Vivosaur",
    "limit": 0,
    "rarity": "",
    "group": "",
    "image": ".\\images\\set\\#.png",
    "element": "",
    "lp": 0,
    "fp": 0,
    "charge-skill": "",
    "range": "",
    "effects": [""],
    "ability": {
        "name": "",
        "effect": ""
    },
    "skill-1": {
        "name": "",
        "cost": 0,
        "damage": 0,
        "effect": ""
    },
    "skill-2": {
        "name": "",
        "cost": 0,
        "damage": 0,
        "effect": ""
    }
}
```

```json
"[Card Number]": {
    "name": "",
    "type": "Action",
    "limit": 0,
    "rarity": "",
    "group": "",
    "image": ".\\images\\set\\#.png",
    "cost": 0,
    "action-type": "",
    "effect": ""
}
```

### Format Notes
#### Vivosaurs
`element`: The valid values are as follows:
| Element |
| ------- |
| Fire    |
| Earth   |
| Air     |
| Water   |
| Neutral |

`range`: The valid values are as follows:
| Range |
| ----- |
| Close |
| Mid   |
| Long  |

`charge-skill`: This is the text describing the Charge Skill of the Vivosaur. It is optional and can be removed for Vivosaurs with no Charge Skill.

`effects`: This is a list of effects the vivosaur has, used for filtering purposes in the deck builder. The valid values are as follows:
| Effect      | Description |
| ----------- | ----------- |
| Discard     | Can cause either player to discard cards from anywhere other than their Deck |
| Draw        | Can cause either player to draw cards |
| Sleep       | Can cause the Sleep status effect |
| Poison      | Can cause the Poison status effect |
| Enrage      | Can cause the Enraged status effect |
| Excite      | Can cause the Excited status effect |
| Scare       | Can cause the Scared status effect |
| Confuse     | Can cause the Confused status effect |
| Enflame     | Can cause the Enflame status effect |
| Harden      | Can cause the Harden status effect |
| Quicken     | Can cause the Quicken status effect |
| Counter     | Can cause the Counter status effect |
| FP Steal    | Has the \[FP Steal\] effect |
| Rest        | Can rest other Vivosaurs |
| Swap        | Can swap Vivosaur zones |
| Mill        | Can cause either player to discard cards directly from their Deck |
| Charge      | Can charge FP into the FP zone |
| Unrest      | Can unrest Vivosaurs, including itself |
| LP Recovery | Can cause Vivosaurs to recover LP, including itself |

`effects` is optional and can be removed if the Vivosaur has none of the above effects.

##### Skills
`damage`: This is optional and can be removed for skills that do no damage. If a skill has non-fixed base damage, set this to 0.
`effect`: This is the text describing the effect of the skill. It is optional and can be removed for skills with no extra effect.
`skill-2`: This is optional and can be removed for Vivosaurs without 2 skills.

#### Actions
`group`: This is optional for Action cards, and can be removed for cards that don't have a group.

#### Both
`[Card Number]`: This **must** be in the format `set-number`. The standard formats for the existing sets are as follows:

(Note that these may change in the future.)
| Set        | Value   |
| ---------- | ------- |
| Set A      | A-##    |
| Set B      | B-##    |
| Mini Set 1 | Deck-## |

`type`: Do not change this. It's set on both templates so that the program knows how to read the data for the card entry.

`limit`: This is optional and can be removed for non-limited cards. Defaults to 3 if removed. I will probably remove this later in favor of an in-game banlist system.

`rarity`: I honestly don't know if this is necessary, but it's listed for all of the cards on Lackey so ¯\\\_(ツ)\_/¯. The field is required for now, but you can leave it blank.

`image`: The path format in the templates is not required, you can change it if you have your image directory set up differently. **ALL** of the image files must be .pngs. The simulator **will** break if they are in any other image format. This may change in the future.