# Umbraco Static Site Generator
This creates a static HTML-file for every node that has a template.
It renders an HTML-file for anything needed.
Current site keeps everything under /website/, publishes under /www/

# Login
Username: demologin@test.com
Password: Password123

## Functions
1. Saves an HTML-file for a node published, overrides an existing
2. Checks old URLs and creates a redirect
3. Removes old HTML-files upon deletion (recursive)

### Todo
1. ~~Saving node upon creation~~
2. ~~Take care of redirects~~
3. ~~Take care of deleted items~~
4. ~~Replace urls with actual urls~~
5. ~~Make more settings variable (custom section in Settings, json config file)~~
6. ~~Check if / has index.html and what to do with it?~~
7. ~~Check if redirect node redirects to this node and update.~~
8. ~~Pull apart from Umbraco, create custom DLL~~
9. Make sure /website/ is not available when not logged in in Umbraco

### Wishlist
1. Publish all nodes to HTML when site starts/has no items in /www/
2. Create nice validation when settings in Umbraco (or settings-file) are not right
3. Create a sitemap.xml upon publish (use .301-files to not index redirects)
4. Upon save, check all other links for the current link and update those (when 301)
5. Create custom Umbraco/Nuget-package
6. Add logging (maybe custom dashboard etc)
7. Remove obsolete references in project