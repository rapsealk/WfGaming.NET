API_VERSION = 'API_v1.0'
MOD_NAME = 'AutoMod'
# Python 2.7

import time     # noqa: E402

# import Keys     # noqa: E402

CALLBACK_TICK = 0.2
callback_handle = None

MAX_HEALTH = 18400
dx, dy = (0, 0)
tick = 0
ROLL_RATE = 10
ribbon_number = 0
"""
keys = {}
keymap = {Keys.KEY_Q: 'Q', Keys.KEY_E: 'E',
          Keys.KEY_W: 'W', Keys.KEY_S: 'S',
          Keys.KEY_A: 'A', Keys.KEY_D: 'D'}
"""


class PlayerInfo:
    def __init__(self, dict_):
        self.id = dict_["id"]
        self.team_id = dict_["teamId"]
        self.ship_id = dict_["shipId"]

        data = dict_["shipGameData"]
        self.health = data["health"] or MAX_HEALTH
        self.health_max = data["maxHealth"] or MAX_HEALTH
        self.yaw = data["yaw"] or 0
        try:
            self.speed = data["speed"] or 0
        except:     # noqa: E722, KeyError
            self.speed = 0

        is_visible = data["isVisible"] or 0
        self.is_visible = bool(is_visible)
        is_ship_visible = data["isShipVisible"] or 0
        self.is_ship_visible = bool(is_ship_visible)

    def __str__(self):
        is_vehicle_burning = battle.isVehicleBurning(self.ship_id)     # noqa: F821, E501
        is_vehicle_flooding = battle.isVehicleFlooding(self.ship_id)   # noqa: F821, E501
        args = (self.id, self.team_id, self.ship_id,
                self.health, self.health_max, self.yaw, self.speed,
                str(self.is_visible).lower(), str(self.is_ship_visible).lower(),    # noqa: E501
                str(bool(is_vehicle_burning)).lower(),
                str(bool(is_vehicle_flooding)).lower())
        return '''{"Id": %d, "TeamId": %d, "ShipId": %s,
                   "Health": %f, "MaxHealth": %d,
                   "Yaw": %f, "Speed": %f,
                   "IsVisible": %s, "IsShipVisible": %s,
                   "IsVehicleBurning": %s, "IsVehicleFlooding": %s}''' % args

    def assign(self, info):
        self.__dict__ = info.__dict__


# def parse_player(dict_):
#     pass
def reset():
    global CACHE_PLAYER, CACHE_ENEMY, dx, dy, tick, ribbon_number
    obj = {
        "id": -1,
        "teamId": -1,
        "shipId": -1,
        "shipGameData": {
            "health": MAX_HEALTH,
            "maxHealth": MAX_HEALTH,
            "yaw": 0,
            "speed": 0,
            "isVisible": False,
            "isShipVisible": False
        }
    }
    CACHE_PLAYER = PlayerInfo(obj)
    CACHE_ENEMY = PlayerInfo(obj)
    with open('enemy.log', 'w') as f:
        f.write(str(CACHE_ENEMY))

    dx, dy = (0, 0)
    tick = 0
    ribbon_number = 0


def callback_func(*args, **kwargs):
    global dx, dy, tick

    player = battle.getSelfPlayerInfo()     # noqa: F821
    # with open('player_raw.log', 'w') as f:
    #     f.write(str(player))

    player = PlayerInfo(player)

    with open('player.log', 'w') as f:
        f.write(str(player))

    # player_ship = battle.getPlayerShipInfo(player.id)    # noqa: F821
    # player_ship["params"]["steerAngle"]
    # with open('player_ship.log', 'w') as f:
    #     f.write(str(player_ship))

    players = battle.getPlayersInfo()           # noqa: F821
    bots = [battle.getPlayerInfo(bot["id"])     # noqa: F821
            for bot in players.values() if bot["id"] != player.id]
    bots = [PlayerInfo(bot) for bot in bots]

    if bots:
        with open('enemy.log', 'w') as f:
            f.write(str(bots[0]))

        # enemy_ship = battle.getPlayerShipInfo(bots[0].id)   # noqa: F821
        # with open('enemy_ship.log', 'w') as f:
        #     f.write(str(enemy_ship))

    dx_, dy_ = (dx, dy)
    dx, dy = (0, 0)
    tick = (tick + 1) % ROLL_RATE
    with open('mouse%d.log' % tick, 'w') as f:
        f.write('{"X": %d, "Y": %d, "timestamp": %d}' % (dx_, dy_, int(time.time() * 1000)))  # noqa: E501


def battle_start(*args, **kwargs):
    global callback_handle

    with open('battle_start.log', 'w') as f:
        f.write('')

    reset()

    callback_handle = callbacks.callback(CALLBACK_TICK, callback_func)  # noqa: F821, E501


def battle_end(*args, **kwargs):
    global callback_handle

    with open('battle_end.log', 'w') as f:
        f.write('%s' % args[0])

    callbacks.cancel(callback_handle)   # noqa: F821
    callback_handle = None


def battle_quit(*args, **kwargs):
    global callback_handle

    with open('battle_quit.log', 'w') as f:
        f.write('')

    callbacks.cancel(callback_handle)   # noqa: F821
    callback_handle = None


def on_mouse_event(event):
    global dx, dy

    if callback_handle is None:
        return

    dx += event.dx
    dy += event.dy


"""
def on_key_event(event):
    # event.isKeyDown()
    # event.isKeyUp()
    # event.isShiftDown()
    # event.isCtrlDown()
    # event.isAltDown()
    key = event.key
    try:
        keys[keymap[key]] = event.isKeyDown()
    except:     # KeyError
        keys[key] = event.isKeyDown()
    try:
        with open('keys.log', 'w') as f:
            f.write(str(keys))
    except:
        pass
"""


def got_ribbon(*args, **kwargs):
    global ribbon_number

    ribbons = list(args)
    ribbon_number += 1
    with open('ribbon%d.log' % ribbon_number, 'w') as f:
        f.write('{"ribbons": %s}' % ribbons)


def on_shell_info(*args, **kwargs):
    """
    victimID - shipID an identifier of the attacked ship
    shooterID - shipID an identifier of the attacking ship
    ammoID - a shell type
    matID - a type of material that was hit
    shootID - ID, a shot identifier
    booleans - a type of damage after taking a hit (this value has a bit mask):
        our ship was damaged
        armor penetration
        damage under water
        the ship is destroyed
        a shell went through the ship
        a ricochet off the armor
        splash damage
        main caliber guns are destroyed
        torpedo launchers are destroyed
        secondary battery is destroyed
    damage - the amount of dealt damage
    shotPosition - a tuple with coordinates of the point of impact
    yaw - yaw of the shell, its angle of impact
    hlinfo - a tuple with the information about the salvo (list with damage info, salvo ID or salvo number)
    """
    ammo_id = args[2]
    ammo = battle.getAmmoParams(ammo_id)    # noqa: F821
    with open('ammo.log', 'w') as f:
        f.write(str(ammo))

    # victim, shooter, damage = args[0], args[1], args[6]
    # args_ = (time.time(), player_ship_id, victim, shooter, damage)
    # shellinfo_log = '{"timestamp": %s, "player_ship_id": %d, "victim": %d, "shooter": %d, "damage": %d}\n' % args_
    # with open('shell_info.log', 'w') as f:
    #     f.write(shellinfo_log)

    booleans = args[5]
    booleans_ = []
    while booleans:
        booleans_.append(booleans % 2)
        booleans = booleans >> 1
    with open('booleans-%d.log' % int(time.time() * 1000), 'w') as f:
        f.write('{"booleans": %s}' % booleans_)


events.onBattleStart(battle_start)      # noqa: F821
events.onBattleEnd(battle_end)          # noqa: F821
events.onBattleQuit(battle_quit)        # noqa: F821
events.onMouseEvent(on_mouse_event)     # noqa: F821
# events.onKeyEvent(on_key_event)         # noqa: F821
# events.onGotRibbon(got_ribbon)          # noqa: F821
# events.onReceiveShellInfo(on_shell_info)    # noqa: F821
