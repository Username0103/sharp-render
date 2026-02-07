- change the processing pipeline from collective stages to indidual task matrix pipelines, where we have one pipeline which we give 1 core to, and assign all the other cores we can to other pipelines, and finishing the pipeline by rendering it without waiting for the others. And, most importantly: you prioritize the cores to go into the pipes with a LRUD prioritization style. This, all in all, makes the UX much better.

- Skip printing foreground pixels which are the color of the user's foreground and vice versa

- Generate each possible ciede2k result into a hashmap at program startup, saving in ProgramData or whatever. To this end, at bootup, add something like:

```
// if cache is already made and found, return

fancyWords: String[len = 10] = ["hypersonic matrices... or something", "like, whatever you want, man.", ...]

{for every 10% of the hashmap made}:

techySoundingThing = fancyWords[rng]

print("Loading {techySoundingThing}...")

{endfor}
```