# Media Meta-Data Manager

Website for managing the metadata of local media (music) files.

This is being developed for the platform Ubuntu Linux with the shell "BASH". 
Other platforms/configurations might incidentally work too.

# Setup & Dependencies 

## mid3v2

This application relies on the program `mid3v2` to access the metadata of media files.
I assume that this `mid3v2` on GitHub at "https://github.com/dhamaniasad/mutagen/blob/master/tools/mid3v2", but I might be incorrect.

To get `mid3v2` install package either `python-mutagen` or `python3-mutagen`, which brings command `mid3v2` 
(e.g. on Ubuntu use `sudo apt-get update -y` and then `sudo apt install python-mutagen`, but if that fails then try `sudo apt install python3-mutagen`).

# Directory

This application looks at media files in the manner of looking at one directory at a time.
This application initially does not know which application you want to work in, thus it asks your computer where your music folder is and uses that.
You may change/override which directory this application uses via this application's "Directory" page.

## Changing your computer's Music directory

### Linux-Ubuntu

In Ubuntu Linux, this can be configured using the instructions at "https://askubuntu.com/questions/113736/how-do-i-set-my-default-music-folder", which instructs to do the following:

1. Edit text file `~/.config/user-dirs.dirs` so that `XDG_MUSIC_DIR` is set to the path of your desired music directory (e.g. `XDG_MUSIC_DIR="/media/username/external_drive/media/music"`).
2. Now you must restart some things for this change to apply:
    1. Restart your computer's shell via keyboard shortcut `ALT` + `F2` to open the command window, and then enter `restart`.
    2. Afterwards restart this application.

#### Persistance after computer restart

If `XDG_MUSIC_DIR` points to a location that is not available when your computer restarts (e.g. an external drive that must be manually remounted), then follow the instructions at "https://unix.stackexchange.com/questions/207216/user-dirs-dirs-reset-at-start-up", which instructs to edit text file `~/.config/user-dirs.conf` so that setting `enabled` equals `False` (i.e. a line of `enabled=False`).
If this file is nonexistant, then create it and add said setting/line into it.

### Windows

Search the web for "move Music folder Windows". Follow those instructions, and then restart this application.
