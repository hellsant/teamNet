var config_module = angular.module('app.config', [])
.constant('MENU_CONFIG',
{
    'ITEMS': [
        {
            'href': '/index.html',
            'icon': 'icon-home',
            'title': 'Home'
        },
        {
            'href': '/App/result/results.html',
            'icon': 'icon-list-alt',
            'title': 'Results'
        },
        {
            'href': '/App/suggestion/suggestions.html',
            'icon': 'icon-star',
            'title': 'Suggestions'
        },
        {
            'href': '/App/requests/requestforreview.html',
            'icon': 'icon-pencil',
            'title': 'Requests'
        },
        {
            'href': '/App/notification/notifications.html',
            'icon': 'icon-envelope',
            'title': 'Notifications'
        },
        {
            'href': '/App/group/groups.html',
            'icon': 'icon-user',
            'title': 'Groups'
        }
    ]
})

.constant('REQUEST_CONFIG',
{
    'VIEW':
        {
            'APPROVE_TITLE': 'Approve',
            'DISSAPPROVE_TITLE': 'Dissapprove'
        },
    'ERRORS': {
        'NETWORK': 'Network error, please try again later'
    }
})

.constant('SHARE_CONFIG',
{
    'VIEW':
        {
            'SHARE_BUTTON': 'Share',
            'CANCEL_BUTTON': 'Cancel',
            'COMMENT_TITLE': 'Comment',
            'SHARE_TITLE': 'Share Task',
        },
    'ERRORS':
        {
        'NETWORK': 'Network error, please try again later'
    }
})
.constant('REQUESTER_CONFIG',
{
    'CONFIRM': 'Are you sure?, will be deleted all your changes',
    'VIEW':
        {
            'SEND_TITLE': 'Send',
            'CANCEL_TITLE': 'Cancel',
            'COMMENT_TITLE': 'Comment',
            'REQUEST_TITLE': 'Request For Review',
            'REQUESTER_MESSAGE': 'Will be reviewed by',
            'REQUESTER_NAME_URL': 'Nick of Reference',
            'REQUESTER_REFERENCE': 'Reference',
            'MAX_LENGTH': 100
        },
    'ERRORS':
        {
        'NETWORK': 'Network error, please try again later',
        'NAME': 'Your need nick for reference',
        'REFERENCE': 'Your need the reference ',
        'NAME_LONG': 'Too long',
        'INVALID_URL': 'URL invalid, try again'
    }
})

.constant('INDEX_CONFIG', //Configurations for index.html and it's controllers.
{
    'VIEW': {
        'TITLE': 'TeamNet',
        'AVATAR_SRC': '/img/default_image.jpg',
        'FIRST_PANEL_TITLE': 'Current Tasks',
        'SECOND_PANEL_TITLE_ONE': 'Task Creator',
        'SECOND_PANEL_TITLE_TWO': 'Task Editor',
        'THIRD_PANEL_TITLE': 'Suggestions',
        'FOURTH_PANEL_TITLE': 'Publications'
    },
    'DEFAULTS_NUM': {
        'DECIMALS': 2
    },
    'LEVELS': [0, 1, 2]
})

.constant('RESULTS_CONFIG', { //Configurations for results.html and it's controllers.
    'INITIAL_PAGE': 1,
    'PAGES_COUNT': 10,
    'MESSAGES': {
        'DIFF_SUGGESTED': 'Your score is too low on this point compared to what you need, you should probably work on it',
        'VALUE_SUGGESTED': 'This point has a high relevance for the level you are at, you should probably work on it',
        'BOTH': 'This point has high relevance for your level and your score is too low, you should work on it',
        'SELECTED': 'tasks added on this point',
        'DEFAULT_TEXT': 'Click the plus button to add tasks on this point and work on it'
    },
    'DECIMAL': 2,
    'MAX_SCORE': 5,
    'MAX_DIFFERENCE': 2,
    'HIGH_PRIORITY': 3,
    'TYPE_1_NAME': 'danger',
    'TYPE_2_NAME': 'warning',
    'TYPE_3_NAME': 'info',
    'TYPE_4_NAME': 'success',
    'TYPE_1_SECONDNAME': 'danger red',
    'TYPE_2_SECONDNAME': 'warning yellow',
    'TYPE_3_SECONDNAME': 'info sky',
    'TYPE_4_SECONDNAME': 'success green',
    'TYPE_1_PERCENTAGE': 0.25,
    'TYPE_2_PERCENTAGE': 0.5,
    'TYPE_3_PERCENTAGE': 0.75,
    'TYPE_4_PERCENTAGE': 1,
    'VIEW': {
        'TITLE': 'TeamNet',
        'AVATAR_SRC': '/img/default_image.jpg',
        'MAIN_TITLE': 'Results',
        'TABLE_TITLE': '360 RESULTS',
        'FIRST_TAB_TITLE': 'Table',
        'SECOND_TAB_TITLE': 'Graphic',
        'FIRST_TAB_TOOLTIP': 'Table View',
        'SECOND_TAB_TOOLTIP': 'Graphic View',
        'FIRST_COLUMN': 'Evaluator',
        'SECOND_COLUMN': 'Result',
        'THIRD_COLUMN': 'Expected',
        'FOURTH_COLUMN': 'Required'
    }
})

.constant('SUG_CONFIG', { //Configurations for suggestions.html and it's controllers.
    'ITEMS_PER_PAGE': 6,
    'INITIAL_PAGE': 1,
    'MAX_SIZE': 4,
    'IMAGES': {
        'JUNIOR': '/img/junior.png',
        'STAFF': '/img/staff.png',
        'SENIOR': '/img/senior.png'
    },
    'VIEW': {
        'TITLE': 'TeamNet',
        'AVATAR_SRC': '/img/default_image.jpg',
        'MAIN_TITLE': 'Suggestions',
        'SECOND_TITLE': 'Create suggestion',
        'THIRD_TITLE': 'Competencies List'
    },
    'ERROR_MESSAGE': 'failure message: ',
    'LEVEL_COUNT': 3
})

.constant('LOGIN_CONFIG', { //All the service's URLs as strings.
    'VIEW': {
        'MAIN_TITLE': 'Login',
        'REGISTER_TITLE': 'Register'
    }
})

.constant('URLs', { //All the service's URLs as strings.
    'SERVICE': 'http://localhost:5859/api/',
    'TASK': 'task',
    'RESULT': 'result',
    'SUGGESTIONS': 'suggestion',
    'USER': 'user',
    'USER_BASIC':'user/basic/',
    'USER_ID': 'user/id',
    'USER_INFO': 'user/userInfo',
    'COMPETENCY': 'competency',
    'TIME': 'time',
    'COMPETENCY_LEVEL': 'competencylevel',
    'LOGIN': 'login',
    'PUBLICATION': 'publication',
    'ROLE':'role',
    'GROUPTASK': 'grouptasks',
    'LOGIN_VIEW_URL': 'http://localhost:4630/index.html',
    'SUGGESTION_VIEW_URL': 'http://localhost:4630/app/suggestion/suggestions.html',
    'REQUEST_REVIEWER': 'requestforreview/reviewer',
    'REQUEST_OWNER': 'requestforreview/owner',
    'REQUEST': 'requestforreview',
    'REFERENCE': 'reference',
    'PUBLICATION': 'publication',
    'LOGIN_URL': 'http://localhost:4630/app/login/login.html',
    'SUGGESTION_VIEW_URL': 'http://localhost:4630/app/suggestion/suggestions.html',
    'GROUPS': 'group',
    'USERROLE': 'userrole',
    'PROFILE_URL': 'http://localhost:4630/app/profile/profile.html'
})

.constant('PROFILE_CONFIG', { //All the profile's URLs as strings.
    'LEVELS': [0, 1, 2],
    'ITEMS_PER_PAGE': 6,
    'INITIAL_PAGE': 1,
    'MAX_SIZE': 4,
    'IMAGES': {
        'JUNIOR': '/img/junior.png',
        'STAFF': '/img/staff.png',
        'SENIOR': '/img/senior.png'
    },
    'VIEW': {
        'TITLE': 'TeamNet',
        'AVATAR_SRC': '/img/default_image.jpg',
        'MAIN_TITLE': 'Profile',
        'FIRST_PANEL_TITLE': 'Tasks',
        'SECOND_PANEL_TITLE': 'New task',
        'THIRD_PANEL_TITLE': 'Task Suggestions',
    },
    'ERROR_MESSAGE': 'failure message: '
})

.constant('GROUPS_CONFIG', { //Constants for the groups window and it's controllers.
    'VIEW': {
        'TITLE': 'TeamNet',
        'AVATAR_SRC': '/img/default_image.jpg',
        'MAIN_TITLE': 'Profile',
        'LIST_TITLE': 'Existing groups',
        'GROUP_INFO_TITLE': 'Group information',
        'EDIT_TITLE': 'Editing group',
        'NEW_TITLE': 'Creating a new group'
    },
    'EDIT': {
        'NAME_PLCHOLDR': 'New name for the group',
        'DESC_PLCHOLDR': 'Description about the new group'
    }
});