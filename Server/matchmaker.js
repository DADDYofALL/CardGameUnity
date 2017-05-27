let packet = require('./packet');

class Match {
	constructor(id){
		this.matchID = id;
		this.playerA = null;
		this.playerB = null;
		this.playerC = null;
		this.playerD = null;
		this.matchStarted = false;
		this.deck = [0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,
		22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,
		42,43,44,45,46,47,48,49,50,51];
		this.card = null;
	}

	canStartMatch(){
		return this.playerA != null && this.playerB != null && this.playerC != null && this.playerD != null;
		//return this.playerA != null && this.playerB != null;
	}

	addPlayer(remote){
		if(this.playerA == null){
			console.log("Match "+this.matchID+": A is waiting");
			this.playerA = remote;
		}else if(this.playerB == null){
			console.log("Match "+this.matchID+": B is joining");
			this.playerB = remote;
		}else if(this.playerC == null){
			console.log("Match "+this.matchID+": C is joining");
			this.playerC = remote;
		}else if(this.playerD == null){
			console.log("Match "+this.matchID+": D is joining");
			this.playerD = remote;
		}
	}

	startMatch(){
		var temp;
		this.matchStarted = true;
		//this.deck = shuffle(this.deck,30);
		var i,j,k,temp;
		for (i = 0; i < 30; i++){
			for (j = 0; j < this.deck.length; j++) {
				k = Math.floor(Math.random() * this.deck.length);
				temp = this.deck[j];
				this.deck[j] = this.deck[k];
				this.deck[k] = temp;
			}
		}
		console.log("Match "+this.matchID+": Start!");
		temp = this.deck.pop()
		this.playerA.send(packet.make_startMatch(this.matchID, 0, this.deck.pop(),this.deck.pop(),this.deck.pop(),this.deck.pop()));
		this.playerB.send(packet.make_startMatch(this.matchID, 1, this.deck.pop(),this.deck.pop(),this.deck.pop(),this.deck.pop()));
		this.playerC.send(packet.make_startMatch(this.matchID, 2, this.deck.pop(),this.deck.pop(),this.deck.pop(),this.deck.pop()));
		this.playerD.send(packet.make_startMatch(this.matchID, 3, this.deck.pop(),this.deck.pop(),this.deck.pop(),this.deck.pop()));
	}

	inMatch(remote){
		return this.playerA == remote || this.playerB == remote || this.playerC == remote || this.playerD == remote;
		//return this.playerA == remote || this.playerB == remote;
	}

	play(remote,card){
		if(!this.matchStarted) return;
		if(this.playerA == remote){
				this.playerB.send(packet.make_oppPlay(card));
				this.playerC.send(packet.make_oppPlay(card));
				this.playerD.send(packet.make_oppPlay(card));
				if (this.deck.Length != 0){
					this.playerA.send(packet.make_Draw(this.deck.pop()));
				}
				console.log('A play, A draw');
		}else if(this.playerB == remote){
				this.playerA.send(packet.make_oppPlay(card));
				this.playerC.send(packet.make_oppPlay(card));
				this.playerD.send(packet.make_oppPlay(card));
				if (this.deck.Length != 0){
					this.playerB.send(packet.make_Draw(this.deck.pop()));
				}
				console.log('B play, B draw');
		}else if(this.playerC == remote){
				this.playerA.send(packet.make_oppPlay(card));
				this.playerB.send(packet.make_oppPlay(card));
				this.playerD.send(packet.make_oppPlay(card));
				if (this.deck.Length != 0){
					this.playerC.send(packet.make_Draw(this.deck.pop()));
				}
				console.log('C play, C draw');

		}else if(this.playerD == remote){
				this.playerA.send(packet.make_oppPlay(card));
				this.playerB.send(packet.make_oppPlay(card));
				this.playerC.send(packet.make_oppPlay(card));
				if (this.deck.Length != 0){
					this.playerD.send(packet.make_Draw(this.deck.pop()));
				}
				console.log('D play, D draw');
		}
	}

	// dead(remote){
	// 	if(remote == this.playerA){
	// 		this.playerB.send(packet.make_gameover(true));
	// 		this.playerC.send(packet.make_gameover(true));
	// 		this.playerD.send(packet.make_gameover(true));
	// 	}else if(remote == this.playerB){
	// 		this.playerA.send(packet.make_gameover(true));
	// 		this.playerC.send(packet.make_gameover(true));
	// 		this.playerD.send(packet.make_gameover(true));
	// 	}else if(remote = this.playerC){
	// 		this.playerA.send(packet.make_gameover(true));
	// 		this.playerB.send(packet.make_gameover(true));
	// 		this.playerD.send(packet.make_gameover(true));
	// 	}else if(remote = this.playerD){
	// 		this.playerA.send(packet.make_gameover(true));
	// 		this.playerB.send(packet.make_gameover(true));
	// 		this.playerC.send(packet.make_gameover(true));
	// 	}
	// 	console.log("Player "+remote+" is dead");
	// }
}

class MatchMaker {

	constructor(){
		this.matches = [];
		this.pendingMatch = null;
		this.counter = 0;
	}

	waitForMatch(remote){
		if(this.pendingMatch == null){
			this.pendingMatch = new Match(this.counter);
			this.counter++;
		}
		if(!this.pendingMatch.canStartMatch()){
			this.pendingMatch.addPlayer(remote);
		}
		if(this.pendingMatch.canStartMatch()){
			this.pendingMatch.startMatch();
			this.matches.push(this.pendingMatch);
			this.pendingMatch = null;
		}
	}

	play(remote,card){
		for(let match of this.matches){
			if(match.inMatch(remote)){
      			match.play(remote,card);
      			break;
      		}
		}
	}

	removeRemote(remote){
		let targetMatch = null;
		for(let match of this.matches){
			if(match.inMatch(remote)){
      			targetMatch = match;
      			break;
      		}
		}
		if(targetMatch != null){
			this.matches.splice(this.matches.indexOf(targetMatch),1);
			targetMatch.disconnect(remote);
		}
	}
}

let matchmaker = new MatchMaker();

module.exports = matchmaker;
