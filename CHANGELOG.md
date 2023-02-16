# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/).

## Unreleased

### Breaking Changes

### Added

### Fixes

### Changes

## 3.2.2

### Added
* Possibility to disable default paging via `WithCustomPaging()`. Paging navigation is then still displayed but the items don't get restricted to the current page and you have to take care of that yourself.

## 3.2.1

### Added
* Property `ItemsCountOverwrite` for the possibility to manually overwrite the grids `ItemsCount` (can be set with new `WithPaging()` overload)
