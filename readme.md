# Umbraco Static Site Generator
This creates a static HTML-file for every node that has a template.
It renders an HTML-file for anything needed.
Current site keeps everything under /website/, publishes under /

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
5. Make more settings variable (create config file)
6. Check if / has index.html and what to do with it?

### Wishlist
1. Publish all nodes to HTML when site starts/has no items in /www/
2. Create a sitemap.xml upon publish (use .301-files to not index redirects)
3. Upon save, check all other links for the current link and update those (when 301)
4. Create custom Umbraco/Nuget-package
5. Add logging (maybe custom dashboard etc)
6. Make sure /website/ is not available when not logged in in Umbraco